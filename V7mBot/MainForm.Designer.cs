namespace V7mBot
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbBotSelection = new System.Windows.Forms.ComboBox();
            this.cbContinuous = new System.Windows.Forms.CheckBox();
            this.linkViewGame = new System.Windows.Forms.LinkLabel();
            this.textProgress = new System.Windows.Forms.Label();
            this.progressTurns = new System.Windows.Forms.ProgressBar();
            this.numericTurns = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textServer = new System.Windows.Forms.TextBox();
            this.btnJoinTraining = new System.Windows.Forms.Button();
            this.btnJoinArena = new System.Windows.Forms.Button();
            this.pictureBoard = new System.Windows.Forms.PictureBox();
            this.pictureChart0 = new System.Windows.Forms.PictureBox();
            this.pictureChart1 = new System.Windows.Forms.PictureBox();
            this.pictureChart2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelChart0 = new System.Windows.Forms.Label();
            this.labelChart1 = new System.Windows.Forms.Label();
            this.labelChart2 = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTurns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureChart0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureChart2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbBotSelection);
            this.panel1.Controls.Add(this.cbContinuous);
            this.panel1.Controls.Add(this.linkViewGame);
            this.panel1.Controls.Add(this.textProgress);
            this.panel1.Controls.Add(this.progressTurns);
            this.panel1.Controls.Add(this.numericTurns);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textServer);
            this.panel1.Controls.Add(this.btnJoinTraining);
            this.panel1.Controls.Add(this.btnJoinArena);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(370, 116);
            this.panel1.TabIndex = 8;
            // 
            // cbBotSelection
            // 
            this.cbBotSelection.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBotSelection.FormattingEnabled = true;
            this.cbBotSelection.Location = new System.Drawing.Point(7, 9);
            this.cbBotSelection.Name = "cbBotSelection";
            this.cbBotSelection.Size = new System.Drawing.Size(140, 22);
            this.cbBotSelection.TabIndex = 19;
            // 
            // cbContinuous
            // 
            this.cbContinuous.AutoSize = true;
            this.cbContinuous.Checked = true;
            this.cbContinuous.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbContinuous.Location = new System.Drawing.Point(95, 42);
            this.cbContinuous.Name = "cbContinuous";
            this.cbContinuous.Size = new System.Drawing.Size(79, 17);
            this.cbContinuous.TabIndex = 20;
            this.cbContinuous.Text = "Continuous";
            this.cbContinuous.UseVisualStyleBackColor = true;
            // 
            // linkViewGame
            // 
            this.linkViewGame.AutoSize = true;
            this.linkViewGame.Enabled = false;
            this.linkViewGame.Location = new System.Drawing.Point(294, 98);
            this.linkViewGame.Name = "linkViewGame";
            this.linkViewGame.Size = new System.Drawing.Size(61, 13);
            this.linkViewGame.TabIndex = 19;
            this.linkViewGame.TabStop = true;
            this.linkViewGame.Text = "View Game";
            this.linkViewGame.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnGameViewLinkClicked);
            // 
            // textProgress
            // 
            this.textProgress.AutoSize = true;
            this.textProgress.Location = new System.Drawing.Point(7, 98);
            this.textProgress.Name = "textProgress";
            this.textProgress.Size = new System.Drawing.Size(30, 13);
            this.textProgress.TabIndex = 18;
            this.textProgress.Text = "0 / 0";
            // 
            // progressTurns
            // 
            this.progressTurns.Location = new System.Drawing.Point(7, 68);
            this.progressTurns.Name = "progressTurns";
            this.progressTurns.Size = new System.Drawing.Size(348, 23);
            this.progressTurns.Step = 1;
            this.progressTurns.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressTurns.TabIndex = 17;
            // 
            // numericTurns
            // 
            this.numericTurns.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericTurns.Location = new System.Drawing.Point(297, 41);
            this.numericTurns.Name = "numericTurns";
            this.numericTurns.Size = new System.Drawing.Size(53, 20);
            this.numericTurns.TabIndex = 16;
            this.numericTurns.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(262, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Turns";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(153, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 19);
            this.label2.TabIndex = 13;
            this.label2.Text = "@";
            // 
            // textServer
            // 
            this.textServer.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textServer.Location = new System.Drawing.Point(177, 9);
            this.textServer.Name = "textServer";
            this.textServer.Size = new System.Drawing.Size(178, 22);
            this.textServer.TabIndex = 11;
            this.textServer.Text = "http://vindinium.org";
            // 
            // btnJoinTraining
            // 
            this.btnJoinTraining.Location = new System.Drawing.Point(177, 39);
            this.btnJoinTraining.Name = "btnJoinTraining";
            this.btnJoinTraining.Size = new System.Drawing.Size(79, 23);
            this.btnJoinTraining.TabIndex = 9;
            this.btnJoinTraining.Text = "Training";
            this.btnJoinTraining.UseVisualStyleBackColor = true;
            this.btnJoinTraining.Click += new System.EventHandler(this.btnJoinTraining_Click);
            // 
            // btnJoinArena
            // 
            this.btnJoinArena.Location = new System.Drawing.Point(7, 39);
            this.btnJoinArena.Name = "btnJoinArena";
            this.btnJoinArena.Size = new System.Drawing.Size(79, 23);
            this.btnJoinArena.TabIndex = 8;
            this.btnJoinArena.Text = "Arena";
            this.btnJoinArena.UseVisualStyleBackColor = true;
            this.btnJoinArena.Click += new System.EventHandler(this.btnJoinArena_Click);
            // 
            // pictureBoard
            // 
            this.pictureBoard.BackColor = System.Drawing.Color.Black;
            this.pictureBoard.Location = new System.Drawing.Point(12, 154);
            this.pictureBoard.Name = "pictureBoard";
            this.pictureBoard.Size = new System.Drawing.Size(120, 120);
            this.pictureBoard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoard.TabIndex = 9;
            this.pictureBoard.TabStop = false;
            // 
            // pictureChart0
            // 
            this.pictureChart0.BackColor = System.Drawing.Color.Black;
            this.pictureChart0.Location = new System.Drawing.Point(136, 154);
            this.pictureChart0.Name = "pictureChart0";
            this.pictureChart0.Size = new System.Drawing.Size(120, 120);
            this.pictureChart0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureChart0.TabIndex = 10;
            this.pictureChart0.TabStop = false;
            // 
            // pictureChart1
            // 
            this.pictureChart1.BackColor = System.Drawing.Color.Black;
            this.pictureChart1.Location = new System.Drawing.Point(262, 154);
            this.pictureChart1.Name = "pictureChart1";
            this.pictureChart1.Size = new System.Drawing.Size(120, 120);
            this.pictureChart1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureChart1.TabIndex = 11;
            this.pictureChart1.TabStop = false;
            // 
            // pictureChart2
            // 
            this.pictureChart2.BackColor = System.Drawing.Color.Black;
            this.pictureChart2.Location = new System.Drawing.Point(388, 154);
            this.pictureChart2.Name = "pictureChart2";
            this.pictureChart2.Size = new System.Drawing.Size(120, 120);
            this.pictureChart2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureChart2.TabIndex = 12;
            this.pictureChart2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Map";
            // 
            // labelChart0
            // 
            this.labelChart0.AutoSize = true;
            this.labelChart0.Location = new System.Drawing.Point(133, 135);
            this.labelChart0.Name = "labelChart0";
            this.labelChart0.Size = new System.Drawing.Size(41, 13);
            this.labelChart0.TabIndex = 15;
            this.labelChart0.Text = "Chart 0";
            // 
            // labelChart1
            // 
            this.labelChart1.AutoSize = true;
            this.labelChart1.Location = new System.Drawing.Point(259, 135);
            this.labelChart1.Name = "labelChart1";
            this.labelChart1.Size = new System.Drawing.Size(41, 13);
            this.labelChart1.TabIndex = 16;
            this.labelChart1.Text = "Chart 1";
            // 
            // labelChart2
            // 
            this.labelChart2.AutoSize = true;
            this.labelChart2.Location = new System.Drawing.Point(385, 135);
            this.labelChart2.Name = "labelChart2";
            this.labelChart2.Size = new System.Drawing.Size(41, 13);
            this.labelChart2.TabIndex = 17;
            this.labelChart2.Text = "Chart 2";
            // 
            // logBox
            // 
            this.logBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logBox.FormattingEnabled = true;
            this.logBox.ItemHeight = 14;
            this.logBox.Location = new System.Drawing.Point(391, 12);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(292, 116);
            this.logBox.TabIndex = 18;
            this.logBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnLogBoxMouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 305);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.labelChart2);
            this.Controls.Add(this.labelChart1);
            this.Controls.Add(this.labelChart0);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureChart2);
            this.Controls.Add(this.pictureChart1);
            this.Controls.Add(this.pictureChart0);
            this.Controls.Add(this.pictureBoard);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "V7m Bot";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTurns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureChart0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureChart2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textServer;
        private System.Windows.Forms.Button btnJoinTraining;
        private System.Windows.Forms.Button btnJoinArena;
        private System.Windows.Forms.NumericUpDown numericTurns;
        private System.Windows.Forms.ProgressBar progressTurns;
        private System.Windows.Forms.LinkLabel linkViewGame;
        private System.Windows.Forms.Label textProgress;
        private System.Windows.Forms.PictureBox pictureBoard;
        private System.Windows.Forms.PictureBox pictureChart0;
        private System.Windows.Forms.PictureBox pictureChart1;
        private System.Windows.Forms.PictureBox pictureChart2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelChart0;
        private System.Windows.Forms.Label labelChart1;
        private System.Windows.Forms.Label labelChart2;
        private System.Windows.Forms.CheckBox cbContinuous;
        private System.Windows.Forms.ListBox logBox;
        private System.Windows.Forms.ComboBox cbBotSelection;
    }
}

