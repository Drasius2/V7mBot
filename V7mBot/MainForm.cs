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
        Connection _con = null;
        Knowledge _knowledge = null;
        Bot _bot = null;
        bool _arena = false;

        public MainForm()
        {
            InitializeComponent();
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
            _con = new Connection(textServer.Text, textKey.Text);
            _con.MoveRequired += OnGameStarted;
            _con.GameFinished += OnGameFinished;
            _con.RequestFailed += OnRequestFailed;
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
            //_bot = new SimpleBot(_knowledge);
            _bot = new MurderBot(_knowledge);
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
            pictureThreat.Image = KnowledgeRenderer.Render(_knowledge.Threat, KnowledgeRenderer.GradientRedToGreen, 4);
            pictureMines.Image = KnowledgeRenderer.Render(_knowledge.Mines, KnowledgeRenderer.GradientGreenToRed, 4);
            pictureTaverns.Image = KnowledgeRenderer.Render(_knowledge.Taverns, KnowledgeRenderer.GradientGreenToRed, 4);
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
