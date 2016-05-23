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
            this.linkViewGame = new System.Windows.Forms.LinkLabel();
            this.textProgress = new System.Windows.Forms.Label();
            this.progressTurns = new System.Windows.Forms.ProgressBar();
            this.numericTurns = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textServer = new System.Windows.Forms.TextBox();
            this.textKey = new System.Windows.Forms.TextBox();
            this.btnJoinTraining = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBoard = new System.Windows.Forms.PictureBox();
            this.pictureThreat = new System.Windows.Forms.PictureBox();
            this.pictureGoals = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTurns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureThreat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureGoals)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.linkViewGame);
            this.panel1.Controls.Add(this.textProgress);
            this.panel1.Controls.Add(this.progressTurns);
            this.panel1.Controls.Add(this.numericTurns);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textServer);
            this.panel1.Controls.Add(this.textKey);
            this.panel1.Controls.Add(this.btnJoinTraining);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(372, 127);
            this.panel1.TabIndex = 8;
            // 
            // linkViewGame
            // 
            this.linkViewGame.AutoSize = true;
            this.linkViewGame.Enabled = false;
            this.linkViewGame.Location = new System.Drawing.Point(308, 98);
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
            this.progressTurns.Size = new System.Drawing.Size(357, 23);
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
            this.numericTurns.Location = new System.Drawing.Point(281, 40);
            this.numericTurns.Name = "numericTurns";
            this.numericTurns.Size = new System.Drawing.Size(83, 20);
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
            this.label3.Location = new System.Drawing.Point(243, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Turns";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Key";
            // 
            // textServer
            // 
            this.textServer.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textServer.Location = new System.Drawing.Point(186, 11);
            this.textServer.Name = "textServer";
            this.textServer.Size = new System.Drawing.Size(178, 22);
            this.textServer.TabIndex = 11;
            this.textServer.Text = "http://vindinium.org";
            // 
            // textKey
            // 
            this.textKey.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textKey.Location = new System.Drawing.Point(38, 11);
            this.textKey.Name = "textKey";
            this.textKey.Size = new System.Drawing.Size(83, 22);
            this.textKey.TabIndex = 10;
            this.textKey.Text = "xdmz43ph";
            // 
            // btnJoinTraining
            // 
            this.btnJoinTraining.Location = new System.Drawing.Point(145, 38);
            this.btnJoinTraining.Name = "btnJoinTraining";
            this.btnJoinTraining.Size = new System.Drawing.Size(96, 23);
            this.btnJoinTraining.TabIndex = 9;
            this.btnJoinTraining.Text = "Training";
            this.btnJoinTraining.UseVisualStyleBackColor = true;
            this.btnJoinTraining.Click += new System.EventHandler(this.btnJoinTraining_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(7, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Arena";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pictureBoard
            // 
            this.pictureBoard.Location = new System.Drawing.Point(12, 145);
            this.pictureBoard.Name = "pictureBoard";
            this.pictureBoard.Size = new System.Drawing.Size(120, 120);
            this.pictureBoard.TabIndex = 9;
            this.pictureBoard.TabStop = false;
            // 
            // pictureThreat
            // 
            this.pictureThreat.Location = new System.Drawing.Point(138, 145);
            this.pictureThreat.Name = "pictureThreat";
            this.pictureThreat.Size = new System.Drawing.Size(120, 120);
            this.pictureThreat.TabIndex = 10;
            this.pictureThreat.TabStop = false;
            // 
            // pictureGoals
            // 
            this.pictureGoals.Location = new System.Drawing.Point(264, 145);
            this.pictureGoals.Name = "pictureGoals";
            this.pictureGoals.Size = new System.Drawing.Size(120, 120);
            this.pictureGoals.TabIndex = 11;
            this.pictureGoals.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 390);
            this.Controls.Add(this.pictureGoals);
            this.Controls.Add(this.pictureThreat);
            this.Controls.Add(this.pictureBoard);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "V7m Bot";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTurns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureThreat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureGoals)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textServer;
        private System.Windows.Forms.TextBox textKey;
        private System.Windows.Forms.Button btnJoinTraining;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericTurns;
        private System.Windows.Forms.ProgressBar progressTurns;
        private System.Windows.Forms.LinkLabel linkViewGame;
        private System.Windows.Forms.Label textProgress;
        private System.Windows.Forms.PictureBox pictureBoard;
        private System.Windows.Forms.PictureBox pictureThreat;
        private System.Windows.Forms.PictureBox pictureGoals;
    }
}

