using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnJoinTraining_Click(object sender, EventArgs e)
        {
            Reset();
            _con = new Connection(textServer.Text, textKey.Text);
            _con.MoveRequired += OnGameStarted;
            _con.GameFinished += OnGameFinished;
            _con.RequestFailed += OnRequestFailed;
            _con.JoinTraining((uint)numericTurns.Value, null);
        }

        private void OnGameFinished(object sender, GameResponse e)
        {
            progressTurns.Value = progressTurns.Maximum;
            //MessageBox.Show("The game has terminaed after " + e.game.turn + " turns!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnRequestFailed(object sender, string e)
        {
            MessageBox.Show(e, "Request failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void OnGameStarted(object sender, GameResponse e)
        {
            progressTurns.Maximum = e.game.maxTurns;
            progressTurns.Value = e.game.turn;
            linkViewGame.Enabled = true;

            _con.MoveRequired -= OnGameStarted; //oneshot
            _con.MoveRequired += OnActionRequired;

            _knowledge = new Knowledge(e);
            _bot = new SimpleBot(_knowledge);
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
            pictureBoard.Image = KnowledgeRenderer.Render(_knowledge.MapState, 5);
            pictureThreat.Image = KnowledgeRenderer.Render(_knowledge.Threat, KnowledgeRenderer.GradientRedToGreen, 5);
            pictureGoals.Image = KnowledgeRenderer.Render(_knowledge.Goals, KnowledgeRenderer.GradientGreenToRed, 5);
        }

        private void Reset()
        {
            if (_con != null)
                _con.Close();
            progressTurns.Value = 0;
            linkViewGame.Enabled = false;
        }

        private void OnGameViewLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(_con != null)
                System.Diagnostics.Process.Start(_con.GameState.viewUrl);
        }
    }
}
