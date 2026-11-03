using System.Drawing.Drawing2D;

namespace FlappyBird.Game;

/// <summary>
/// Aralarında geçiş boşluğu olan alt-üst boru çifti.
/// Ekran dışına çıkınca yok edilmez; <see cref="Reposition"/> ile sağa taşınıp yeniden kullanılır.
/// </summary>
public sealed class PipePair : GameObject
{
    public const float PipeWidth = 70f;
    private const float LipHeight = 22f;
    private const float LipOverhang = 5f;

    private readonly float _groundTop;

    public float GapTop { get; private set; }
    public float GapSize { get; private set; }
    public float GapBottom => GapTop + GapSize;

    /// <summary>Hareket hızı oyun motoru tarafından belirlenir (seviye arttıkça değişir).</summary>
    public float Speed { get; set; }

    /// <summary>Bu engelden geçildiği için skor verildi mi? Aynı engelden iki kez puan alınmasını önler.</summary>
    public bool IsPassed { get; private set; }

    public RectangleF TopPipe => new(X, 0, Width, GapTop);
    public RectangleF BottomPipe => new(X, GapBottom, Width, _groundTop - GapBottom);

    public PipePair(float x, float gapTop, float gapSize, float groundTop)
        : base(x, 0, PipeWidth, groundTop)
    {
        _groundTop = groundTop;
        GapTop = gapTop;
        GapSize = gapSize;
    }

    public override void Update() => X -= Speed;

    public void Reposition(float x, float gapTop, float gapSize)
    {
        X = x;
        GapTop = gapTop;
        GapSize = gapSize;
        IsPassed = false;
    }

    public void MarkPassed() => IsPassed = true;

    public bool CollidesWith(RectangleF rectangle) =>
        TopPipe.IntersectsWith(rectangle) || BottomPipe.IntersectsWith(rectangle);

    public override void Draw(Graphics graphics)
    {
        DrawPipe(graphics, TopPipe, lipAtBottom: true);
        DrawPipe(graphics, BottomPipe, lipAtBottom: false);
    }

    private static void DrawPipe(Graphics graphics, RectangleF pipe, bool lipAtBottom)
    {
        if (pipe.Height <= 0)
        {
            return;
        }

        float lipY = lipAtBottom ? pipe.Bottom - LipHeight : pipe.Top;
        var lip = new RectangleF(
            pipe.X - LipOverhang,
            lipY,
            pipe.Width + 2 * LipOverhang,
            LipHeight);

        using var bodyBrush = new LinearGradientBrush(
            new RectangleF(pipe.X, 0, pipe.Width, 1),
            Color.FromArgb(134, 239, 172),
            Color.FromArgb(21, 128, 61),
            LinearGradientMode.Horizontal);
        using var lipBrush = new LinearGradientBrush(
            new RectangleF(lip.X, 0, lip.Width, 1),
            Color.FromArgb(134, 239, 172),
            Color.FromArgb(22, 101, 52),
            LinearGradientMode.Horizontal);
        using var outline = new Pen(Color.FromArgb(20, 83, 45), 2f);

        graphics.FillRectangle(bodyBrush, pipe);
        graphics.DrawRectangle(outline, pipe.X, pipe.Y, pipe.Width, pipe.Height);
        graphics.FillRectangle(lipBrush, lip);
        graphics.DrawRectangle(outline, lip.X, lip.Y, lip.Width, lip.Height);
    }
}
