namespace FlappyBird;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        gamePanel = new GamePanel();
        gameTimer = new System.Windows.Forms.Timer(components);
        lblScoreCaption = new Label();
        lblScoreValue = new Label();
        lblHighScoreCaption = new Label();
        lblHighScoreValue = new Label();
        lblLevelCaption = new Label();
        lblLevelValue = new Label();
        lblSpeedCaption = new Label();
        lblSpeedValue = new Label();
        lblStateCaption = new Label();
        lblStateValue = new Label();
        lblDifficulty = new Label();
        cmbDifficulty = new ComboBox();
        chkSound = new CheckBox();
        btnNewGame = new Button();
        lblHelp = new Label();
        lblNotice = new Label();
        SuspendLayout();
        //
        // gamePanel
        //
        gamePanel.Location = new Point(0, 0);
        gamePanel.Name = "gamePanel";
        gamePanel.Size = new Size(480, 640);
        gamePanel.TabIndex = 0;
        //
        // gameTimer
        //
        gameTimer.Interval = 16;
        gameTimer.Tick += GameTimer_Tick;
        //
        // lblScoreCaption
        //
        lblScoreCaption.AutoSize = true;
        lblScoreCaption.Location = new Point(496, 20);
        lblScoreCaption.Name = "lblScoreCaption";
        lblScoreCaption.Text = "Skor";
        //
        // lblScoreValue
        //
        lblScoreValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblScoreValue.Location = new Point(496, 40);
        lblScoreValue.Name = "lblScoreValue";
        lblScoreValue.Size = new Size(190, 36);
        lblScoreValue.Text = "0";
        //
        // lblHighScoreCaption
        //
        lblHighScoreCaption.AutoSize = true;
        lblHighScoreCaption.Location = new Point(496, 86);
        lblHighScoreCaption.Name = "lblHighScoreCaption";
        lblHighScoreCaption.Text = "En Yüksek Skor";
        //
        // lblHighScoreValue
        //
        lblHighScoreValue.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblHighScoreValue.Location = new Point(496, 106);
        lblHighScoreValue.Name = "lblHighScoreValue";
        lblHighScoreValue.Size = new Size(190, 36);
        lblHighScoreValue.Text = "0";
        //
        // lblLevelCaption
        //
        lblLevelCaption.AutoSize = true;
        lblLevelCaption.Location = new Point(496, 152);
        lblLevelCaption.Name = "lblLevelCaption";
        lblLevelCaption.Text = "Seviye";
        //
        // lblLevelValue
        //
        lblLevelValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblLevelValue.Location = new Point(496, 172);
        lblLevelValue.Name = "lblLevelValue";
        lblLevelValue.Size = new Size(190, 26);
        lblLevelValue.Text = "1";
        //
        // lblSpeedCaption
        //
        lblSpeedCaption.AutoSize = true;
        lblSpeedCaption.Location = new Point(496, 206);
        lblSpeedCaption.Name = "lblSpeedCaption";
        lblSpeedCaption.Text = "Hız";
        //
        // lblSpeedValue
        //
        lblSpeedValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblSpeedValue.Location = new Point(496, 226);
        lblSpeedValue.Name = "lblSpeedValue";
        lblSpeedValue.Size = new Size(190, 26);
        lblSpeedValue.Text = "0";
        //
        // lblStateCaption
        //
        lblStateCaption.AutoSize = true;
        lblStateCaption.Location = new Point(496, 260);
        lblStateCaption.Name = "lblStateCaption";
        lblStateCaption.Text = "Durum";
        //
        // lblStateValue
        //
        lblStateValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblStateValue.Location = new Point(496, 280);
        lblStateValue.Name = "lblStateValue";
        lblStateValue.Size = new Size(190, 26);
        lblStateValue.Text = "Hazır";
        //
        // lblDifficulty
        //
        lblDifficulty.AutoSize = true;
        lblDifficulty.Location = new Point(496, 322);
        lblDifficulty.Name = "lblDifficulty";
        lblDifficulty.Text = "Zorluk";
        //
        // cmbDifficulty
        //
        cmbDifficulty.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDifficulty.Location = new Point(496, 342);
        cmbDifficulty.Name = "cmbDifficulty";
        cmbDifficulty.Size = new Size(190, 23);
        cmbDifficulty.TabStop = false;
        cmbDifficulty.SelectionChangeCommitted += CmbDifficulty_SelectionChangeCommitted;
        //
        // chkSound
        //
        chkSound.AutoSize = true;
        chkSound.Location = new Point(496, 376);
        chkSound.Name = "chkSound";
        chkSound.TabStop = false;
        chkSound.Text = "Ses efektleri";
        chkSound.UseVisualStyleBackColor = true;
        //
        // btnNewGame
        //
        btnNewGame.Location = new Point(496, 408);
        btnNewGame.Name = "btnNewGame";
        btnNewGame.Size = new Size(190, 40);
        btnNewGame.TabStop = false;
        btnNewGame.Text = "Yeni Oyun";
        btnNewGame.UseVisualStyleBackColor = true;
        btnNewGame.Click += BtnNewGame_Click;
        //
        // lblHelp
        //
        lblHelp.Location = new Point(496, 464);
        lblHelp.Name = "lblHelp";
        lblHelp.Size = new Size(190, 100);
        lblHelp.Text = "Boşluk / Yukarı Ok: zıpla\r\nR / Enter: oyun bitince yeniden başlat\r\n\r\nHer 5 puanda seviye ve hız artar.";
        //
        // lblNotice
        //
        lblNotice.ForeColor = Color.Firebrick;
        lblNotice.Location = new Point(496, 570);
        lblNotice.Name = "lblNotice";
        lblNotice.Size = new Size(190, 56);
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(702, 640);
        Controls.Add(gamePanel);
        Controls.Add(lblScoreCaption);
        Controls.Add(lblScoreValue);
        Controls.Add(lblHighScoreCaption);
        Controls.Add(lblHighScoreValue);
        Controls.Add(lblLevelCaption);
        Controls.Add(lblLevelValue);
        Controls.Add(lblSpeedCaption);
        Controls.Add(lblSpeedValue);
        Controls.Add(lblStateCaption);
        Controls.Add(lblStateValue);
        Controls.Add(lblDifficulty);
        Controls.Add(cmbDifficulty);
        Controls.Add(chkSound);
        Controls.Add(btnNewGame);
        Controls.Add(lblHelp);
        Controls.Add(lblNotice);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        KeyPreview = true;
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Flappy Bird - YMH219";
        KeyDown += MainForm_KeyDown;
        KeyUp += MainForm_KeyUp;
        ResumeLayout(false);
        PerformLayout();
    }

    private GamePanel gamePanel;
    private System.Windows.Forms.Timer gameTimer;
    private Label lblScoreCaption;
    private Label lblScoreValue;
    private Label lblHighScoreCaption;
    private Label lblHighScoreValue;
    private Label lblLevelCaption;
    private Label lblLevelValue;
    private Label lblSpeedCaption;
    private Label lblSpeedValue;
    private Label lblStateCaption;
    private Label lblStateValue;
    private Label lblDifficulty;
    private ComboBox cmbDifficulty;
    private CheckBox chkSound;
    private Button btnNewGame;
    private Label lblHelp;
    private Label lblNotice;
}
