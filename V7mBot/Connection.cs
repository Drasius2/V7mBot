using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace V7mBot
{
    public class Connection
    {
        string _serverURL;
        string _botKey;
        bool _closed;

        GameResponse _gameState;

        public GameResponse GameState
        {
            get
            {
                return _gameState;
            }
        }

        public event EventHandler<string> RequestFailed;
        public event EventHandler<GameResponse> MoveRequired;
        public event EventHandler<GameResponse> GameFinished;

        public Connection(string server, string botKey)
        {
            _serverURL = server;
            _botKey = botKey;
            _closed = false;
        }

        public void JoinTraining(uint numTurns, string map)
        {            
            string parameters = "key=" + _botKey + "&turns=" + numTurns;
            if (!string.IsNullOrEmpty(map))
                parameters += "&map=" + map;

            Uri uri = new Uri(_serverURL + "/api/training");
            SendRequest(uri, parameters);
        }

        public void Close()
        {
            _closed = true;
        }


        public void JoinArena(string botKey)
        {
            //uri = serverURL + "/api/arena";
        }

        public void SendMove(Move action)
        {
            string parameters = "key=" + _botKey + "&dir=" + action.ToString();
            Uri uri = new Uri(_gameState.playUrl);
            SendRequest(uri, parameters);
        }

        private void SendRequest(Uri uri, string parameters)
        {
            if (_closed)
                return;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                try
                {                    
                    client.UploadStringCompleted += OnRequestCompleted;
                    client.UploadStringAsync(uri, parameters);
                }
                catch (WebException exception)
                {
                    using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                    {
                        string errorText = reader.ReadToEnd();
                        if (!_closed)
                            RequestFailed(this, errorText);
                    }
                }
            }
        }

        private void OnRequestCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (_closed)
                return;

            if (e.Error != null)
            {
                if(e.Error.InnerException != null)
                    RequestFailed(this, e.Error.InnerException.Message);
                else
                    RequestFailed(this, e.Error.Message);
            }
            else
            {
                _gameState = Deserialize(e.Result);
                if (_gameState.game.finished)
                    GameFinished(this, _gameState);
                else
                    MoveRequired(this, _gameState);
            }
        }

        private GameResponse Deserialize(string json)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(GameResponse));
            return (GameResponse)ser.ReadObject(stream);
        }

    }
}
