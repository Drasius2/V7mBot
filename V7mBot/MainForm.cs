using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using V7mBot.Knowledge;

namespace V7mBot
{
    public partial class MainForm : Form
    {
        Connection _con = null;
        KnowledgeBase _knowledge = null;

        Random _rng = new Random();
        T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_rng.Next(v.Length));
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnJoinTraining_Click(object sender, EventArgs e)
        {
            ResetUI();
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

            _knowledge = new KnowledgeBase(e);
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
            Connection.Move action = RandomEnumValue<Connection.Move>();
            _con.SendMove(action);
            pictureBoard.Image = KnowledgeRenderer.RenderMap(_knowledge.MapState, 4);
        }

        private void ResetUI()
        {
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
