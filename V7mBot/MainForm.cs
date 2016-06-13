using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using V7mBot.AI;
using V7mBot.AI.Bots;

namespace V7mBot
{
    public partial class MainForm : Form
    {
        struct BotAccount
        {
            public BotAccount(string name, string key, Type bot)
            {
                Name = name;
                Key = key;
                Bot = bot;
            }
            public readonly Type Bot;
            public readonly string Key;
            public readonly string Name;
        }
        List<BotAccount> _accounts = new List<BotAccount>()
        {
            new BotAccount("Simpleton", "0r9s62d9", typeof(PeonBot)),
            new BotAccount("Rascal", "7q42tooa", typeof(GruntBot)),
            new BotAccount("Bitsquid", "xdmz43ph", typeof(RaiderBot))
        };

        struct VisControls
        {
            public Label Description;
            public PictureBox Image;
        }
        List<VisControls> _visCtrls = new List<VisControls>();

        Connection _con = null;
        Knowledge _knowledge = null;
        BotAccount _account;
        Bot _bot = null;
        bool _arena = false;

        public MainForm()
        {
            InitializeComponent();
            foreach (var entry in _accounts)
                cbBotSelection.Items.Add(entry.Name);
            cbBotSelection.SelectedIndex = 0;

            //add the controls dynamically
            _visCtrls.Add(new VisControls() { Description = labelChart0, Image = pictureChart0 });
            _visCtrls.Add(new VisControls() { Description = labelChart1, Image = pictureChart1 });
            _visCtrls.Add(new VisControls() { Description = labelChart2, Image = pictureChart2 });
        }

        private void btnJoinTraining_Click(object sender, EventArgs e)
        {
            _arena = false;
            NewCon();
            _con.JoinTraining((uint)numericTurns.Value, null);
        }

        private void btnJoinArena_Click(object sender, EventArgs e)
        {
            _arena = true;
            NewCon();
            _con.JoinArena();
        }

        private void NewCon()
        {
            progressTurns.Value = 0;
            linkViewGame.Enabled = false;

            if (_con != null)
                _con.Close();

            Log("Waiting for next game...");
            _account = GetSelectedAccount();
            _con = new Connection(textServer.Text, _account.Key);
            _con.MoveRequired += OnGameStarted;
            _con.GameFinished += OnGameFinished;
            _con.RequestFailed += OnRequestFailed;
        }

        private BotAccount GetSelectedAccount()
        {
            string name = cbBotSelection.SelectedItem as string;
            BotAccount account = _accounts.First(acc => acc.Name == name);
            return account;
        }

        private void OnGameFinished(object sender, GameResponse e)
        {
            progressTurns.Value = progressTurns.Maximum;
            //write result into log
            LogResult(e);
            Restart();
        }

        private void OnRequestFailed(object sender, string e)
        {
            Log("ERROR: " + e);
            Restart();
        }

        private void OnGameStarted(object sender, GameResponse e)
        {
            Log("Joined " + _con.GameState.viewUrl);

            progressTurns.Maximum = e.game.maxTurns;
            progressTurns.Value = e.game.turn;
            linkViewGame.Enabled = true;

            _con.MoveRequired -= OnGameStarted; //oneshot
            _con.MoveRequired += OnActionRequired;

            _knowledge = new Knowledge(e);
            _bot = Activator.CreateInstance(_account.Bot, _knowledge) as Bot;
            PlayMove(e);
        }

        private void OnActionRequired(object sender, GameResponse e)
        {
            progressTurns.Value = e.game.turn;
            textProgress.Text = e.game.turn + " / " + e.game.maxTurns;
            PlayMove(e);
        }

        private void PlayMove(GameResponse rawData)
        {
            _knowledge.Update(rawData);
            Move action = _bot.Act();
            _con.SendMove(action);
            pictureBoard.Image = KnowledgeRenderer.Render(_knowledge.Map, 4);

            var visRequests = _bot.Visualizaton.ToList();
            int max = Math.Min(visRequests.Count, _visCtrls.Count);
            for(int i = 0; i < max; i++)
            {
                _visCtrls[i].Description.Text = visRequests[i].Description;
                _visCtrls[i].Image.Image = KnowledgeRenderer.Render(_knowledge[visRequests[i].ChartName], KnowledgeRenderer.GradientRedToGreen, 4);
            }
            for(int i = max; i < _visCtrls.Count; i++)
            {
                _visCtrls[i].Description.Text = "...";
                _visCtrls[i].Image.Image = null;
            }
        }


        private void Restart()
        {
            if (cbContinuous.Checked && _arena)
            {
                NewCon();
                _con.JoinArena();
            }
        }

        private void LogResult(GameResponse e)
        {
            Log("***Game Over***");
            int place = 1;
            foreach (var hero in e.game.heroes.OrderByDescending(h => h.gold))
                Log((place++) + ". " + hero.name + " with " + hero.gold + " gold.");
            Debug.WriteLine("***************");
        }

        private void Log(string msg)
        {
            logBox.Items.Add(msg);
        }

        private void OnGameViewLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(_con != null)
                Process.Start(_con.GameState.viewUrl);
        }

        private void OnLogBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            string msg = logBox.SelectedItem as string;
            if (msg != null && msg.Contains("http://"))
            {
                string url = msg.Substring(msg.IndexOf("http://"));
                Process.Start(url);
            }
        }
    }
}
