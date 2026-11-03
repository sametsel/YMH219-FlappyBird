using System.Diagnostics;
using FlappyBird.Game;

namespace FlappyBird;

public partial class MainForm : Form
{
    private const int FieldWidth = 480;
    private const int FieldHeight = 640;
    private const int GroundHeight = 70;

    private const double StepMilliseconds = 16.0; // Bir oyun adımının temsil ettiği süre
    private const int MaxStepsPerFrame = 5;       // Uzun bir takılmadan sonra oyun aniden sıçramasın

    private readonly GameEngine _engine;
    private readonly HighScoreStore _highScoreStore = new();
    private readonly Stopwatch _clock = new();
    private double _pendingMilliseconds;
    private int _highScore;
    private bool _flapKeyHeld; // Tuş basılı tutulunca art arda zıplamayı engeller

    public MainForm()
    {
        InitializeComponent();

        _engine = new GameEngine(FieldWidth, FieldHeight, GroundHeight);
        _engine.Scored += Engine_Scored;
        _engine.Crashed += Engine_Crashed;
        gamePanel.Engine = _engine;

        _highScore = _highScoreStore.Load();

        cmbDifficulty.Items.AddRange(DifficultyLevel.All.Cast<object>().ToArray());
        cmbDifficulty.SelectedItem = DifficultyLevel.Normal;

        UpdateHud();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        gamePanel.Focus();
        _clock.Start();
        gameTimer.Start();
    }

    /// <summary>
    /// Sabit adımlı oyun döngüsü. Windows zamanlayıcısı tam 16 ms'de bir tetiklenmeyebilir;
    /// bu yüzden geçen gerçek süre biriktirilir ve o süreye denk gelen sayıda adım çalıştırılır.
    /// Böylece oyun hızı bilgisayarın yoğunluğundan etkilenmez.
    /// </summary>
    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        _pendingMilliseconds += _clock.Elapsed.TotalMilliseconds;
        _clock.Restart();

        int steps = 0;
        while (_pendingMilliseconds >= StepMilliseconds && steps < MaxStepsPerFrame)
        {
            _engine.Tick();
            _pendingMilliseconds -= StepMilliseconds;
            steps++;
        }

        // Yetişilemeyen süre atılır; aksi halde sonraki karelerde birikip oyunu hızlandırırdı.
        if (steps == MaxStepsPerFrame)
        {
            _pendingMilliseconds = 0;
        }

        gamePanel.Invalidate();
        UpdateHud();
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Space:
            case Keys.Up:
                if (!_flapKeyHeld)
                {
                    _flapKeyHeld = true;
                    _engine.Flap();
                }
                e.SuppressKeyPress = true;
                break;

            case Keys.R:
            case Keys.Enter:
                // Akan oyun yanlışlıkla sıfırlanmasın; yalnızca bitmişken yeniden başlatır.
                if (_engine.State == GameState.GameOver)
                {
                    StartNewGame();
                }
                e.SuppressKeyPress = true;
                break;
        }
    }

    private void MainForm_KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Space or Keys.Up)
        {
            _flapKeyHeld = false;
            e.Handled = true;
        }
    }

    private void BtnNewGame_Click(object? sender, EventArgs e) => StartNewGame();

    private void CmbDifficulty_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        if (cmbDifficulty.SelectedItem is DifficultyLevel difficulty)
        {
            _engine.SetDifficulty(difficulty);
            UpdateHud();
        }
        gamePanel.Focus();
    }

    private void Engine_Scored(object? sender, EventArgs e)
    {
        if (chkSound.Checked)
        {
            PlayTone(880, 60);
        }
    }

    private void Engine_Crashed(object? sender, EventArgs e)
    {
        if (chkSound.Checked)
        {
            PlayTone(180, 250);
        }

        if (_engine.Score > _highScore)
        {
            _highScore = _engine.Score;
            if (!_highScoreStore.Save(_highScore))
            {
                lblNotice.Text = "En yüksek skor dosyaya yazılamadı.";
            }
        }
    }

    private void StartNewGame()
    {
        _engine.Reset();
        _flapKeyHeld = false;
        lblNotice.Text = string.Empty;
        UpdateHud();
        gamePanel.Invalidate();
        gamePanel.Focus();
    }

    private void UpdateHud()
    {
        SetTextIfChanged(lblScoreValue, _engine.Score.ToString());
        SetTextIfChanged(lblHighScoreValue, Math.Max(_highScore, _engine.Score).ToString());
        SetTextIfChanged(lblLevelValue, _engine.Level.ToString());
        SetTextIfChanged(lblSpeedValue, _engine.Speed.ToString("0.0"));
        SetTextIfChanged(lblStateValue, _engine.State switch
        {
            GameState.Ready => "Hazır",
            GameState.Running => "Oyunda",
            GameState.GameOver => "Game Over",
            _ => string.Empty,
        });

        // Zorluk yalnızca oyun akmıyorken değiştirilebilir.
        bool canChangeDifficulty = _engine.State != GameState.Running;
        if (cmbDifficulty.Enabled != canChangeDifficulty)
        {
            cmbDifficulty.Enabled = canChangeDifficulty;
        }
    }

    // Etiket metnini gereksiz yere atamamak saniyede 60 kez yeniden çizimi önler.
    private static void SetTextIfChanged(Label label, string text)
    {
        if (label.Text != text)
        {
            label.Text = text;
        }
    }

    // Console.Beep çalarken bekletir; oyun döngüsü takılmasın diye ayrı görevde çalınır.
    private static void PlayTone(int frequency, int duration) =>
        Task.Run(() => Console.Beep(frequency, duration));
}
