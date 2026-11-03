namespace FlappyBird.Game;

/// <summary>
/// Oyunun kurallarını ve durumunu yönetir: skor, seviye, hız, çarpışma ve engellerin yeniden kullanımı.
/// Arayüzden bağımsızdır; form yalnızca Tick/Flap çağırır ve olayları dinler.
/// </summary>
public sealed class GameEngine
{
    public const int PipeCount = 3;
    public const float PipeSpacing = 260f;
    public const int PointsPerLevel = 5;

    private const float BirdStartX = 90f;
    private const float FirstPipeOffset = 80f;
    private const float SpeedPerLevel = 0.3f;
    private const float MaxSpeed = 6f;
    private const float GapMargin = 50f; // Boşluğun tavan ve zemine bırakacağı en az mesafe

    // Art arda iki boşluk arasındaki en büyük yükseklik farkı. En yüksek hızda bile kuşun
    // iki engel arasında tırmanabileceği mesafenin altında tutulur; aksi halde geçilemez engel çıkardı.
    private const float MaxGapShift = 130f;

    private readonly Random _random;
    private readonly List<PipePair> _pipes = new();

    public int FieldWidth { get; }
    public int FieldHeight { get; }
    public float GroundTop { get; }

    public GameState State { get; private set; } = GameState.Ready;
    public DifficultyLevel Difficulty { get; private set; } = DifficultyLevel.Normal;
    public Bird Bird { get; private set; }
    public IReadOnlyList<PipePair> Pipes => _pipes;
    public int Score { get; private set; }
    public int Level => 1 + Score / PointsPerLevel;
    public float Speed => Math.Min(Difficulty.BaseSpeed + (Level - 1) * SpeedPerLevel, MaxSpeed);

    /// <summary>Skor arttığında tetiklenir.</summary>
    public event EventHandler? Scored;

    /// <summary>Kuş engele veya zemine çarptığında tetiklenir.</summary>
    public event EventHandler? Crashed;

    public GameEngine(int fieldWidth, int fieldHeight, int groundHeight, Random? random = null)
    {
        FieldWidth = fieldWidth;
        FieldHeight = fieldHeight;
        GroundTop = fieldHeight - groundHeight;
        _random = random ?? new Random();
        Bird = CreateBird();
        Reset();
    }

    /// <summary>Yeni oyun: tüm durum başlangıç değerlerine döner.</summary>
    public void Reset()
    {
        State = GameState.Ready;
        Score = 0;
        Bird = CreateBird();

        _pipes.Clear();
        float gapTop = (GroundTop - Difficulty.GapSize) / 2f; // İlk boşluk ortadan başlar.
        for (int i = 0; i < PipeCount; i++)
        {
            float x = FieldWidth + FirstPipeOffset + i * PipeSpacing;
            _pipes.Add(new PipePair(x, gapTop, Difficulty.GapSize, GroundTop));
            gapTop = NextGapTop(gapTop);
        }
    }

    /// <summary>Zorluk değişince oyun yeni ayarlarla baştan kurulur.</summary>
    public void SetDifficulty(DifficultyLevel difficulty)
    {
        Difficulty = difficulty ?? throw new ArgumentNullException(nameof(difficulty));
        Reset();
    }

    /// <summary>Zıplama komutu. Hazır durumdaysa aynı zamanda oyunu başlatır.</summary>
    public void Flap()
    {
        switch (State)
        {
            case GameState.Ready:
                State = GameState.Running;
                Bird.Flap();
                break;
            case GameState.Running:
                Bird.Flap();
                break;
            case GameState.GameOver:
                // Kazara devam etmemek için oyun bitince zıplama yok sayılır; Yeni Oyun gerekir.
                break;
        }
    }

    /// <summary>Oyun döngüsünün bir adımı; form içindeki Timer her tetiklendiğinde çağrılır.</summary>
    public void Tick()
    {
        if (State != GameState.Running)
        {
            return;
        }

        Bird.Update();
        Bird.ClampToCeiling();

        foreach (PipePair pipe in _pipes)
        {
            pipe.Speed = Speed;
            pipe.Update();
        }

        RecyclePipes();
        UpdateScore();
        CheckCollisions();
    }

    private Bird CreateBird() => new(BirdStartX, (GroundTop - 28f) / 2f);

    /// <summary>Önceki boşluğa göre, sınırlı bir kayma ile yeni boşluk konumu seçer.</summary>
    private float NextGapTop(float previousGapTop)
    {
        float lowest = Math.Max(GapMargin, previousGapTop - MaxGapShift);
        float highest = Math.Min(GroundTop - Difficulty.GapSize - GapMargin, previousGapTop + MaxGapShift);
        return lowest + (float)_random.NextDouble() * (highest - lowest);
    }

    /// <summary>Ekranın solundan çıkan engeli en sağdaki engelin arkasına taşır.</summary>
    private void RecyclePipes()
    {
        foreach (PipePair pipe in _pipes)
        {
            if (pipe.Right < 0)
            {
                PipePair rightmost = _pipes.MaxBy(p => p.X)!;
                pipe.Reposition(rightmost.X + PipeSpacing, NextGapTop(rightmost.GapTop), Difficulty.GapSize);
            }
        }
    }

    private void UpdateScore()
    {
        foreach (PipePair pipe in _pipes)
        {
            if (!pipe.IsPassed && pipe.Right < Bird.X)
            {
                pipe.MarkPassed();
                Score++;
                Scored?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void CheckCollisions()
    {
        if (Bird.Y + Bird.Height >= GroundTop)
        {
            Bird.RestOn(GroundTop);
            EndGame();
            return;
        }

        RectangleF hitbox = Bird.Hitbox;
        if (_pipes.Any(pipe => pipe.CollidesWith(hitbox)))
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        State = GameState.GameOver;
        Crashed?.Invoke(this, EventArgs.Empty);
    }
}
