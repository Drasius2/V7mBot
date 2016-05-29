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
            new BotAccount("Rascal", "7q42tooa", typeof(GruntBot))
        };

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
            //_bot = new SimpleBot(_knowledge);
            //_bot = new MurderBot(_knowledge);
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
            pictureThreat.Image = KnowledgeRenderer.Render(_knowledge["threat"], KnowledgeRenderer.GradientRedToGreen, 4);
            pictureMines.Image = KnowledgeRenderer.Render(_knowledge["mines"], KnowledgeRenderer.GradientGreenToRed, 4);
            pictureTaverns.Image = KnowledgeRenderer.Render(_knowledge["taverns"], KnowledgeRenderer.GradientGreenToRed, 4);
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
