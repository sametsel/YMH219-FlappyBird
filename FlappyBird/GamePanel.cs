using System.ComponentModel;
using System.Drawing.Drawing2D;
using FlappyBird.Game;

namespace FlappyBird;

/// <summary>
/// Oyun alanını çizen kontrol. Titremeyi önlemek için çift tamponlama kullanır
/// ve klavye odağını alabilir; böylece Boşluk ve yön tuşları düğmelere gitmez.
/// </summary>
public sealed class GamePanel : Control
{
    private GameEngine? _engine;
    private float _groundScroll;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public GameEngine? Engine
    {
        get => _engine;
        set
        {
            _engine = value;
            Invalidate();
        }
    }

    public GamePanel()
    {
        SetStyle(
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.ResizeRedraw |
            ControlStyles.Selectable,
            true);
    }

    // Bu tuşlar normalde "diyalog tuşu" sayılır ve odak değiştirir; oyun için normal girdi yapıyoruz.
    protected override bool IsInputKey(Keys keyData) =>
        keyData is Keys.Up or Keys.Down or Keys.Left or Keys.Right or Keys.Space or Keys.Tab
        || base.IsInputKey(keyData);

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (_engine is null)
        {
            return;
        }

        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // Oyun sabit koordinatlarla hesaplanır. Windows ekran ölçeği %100'den büyükse kontrol büyür;
        // çizim de aynı oranda ölçeklenir ki oyun alanı kontrolü tam doldursun.
        g.ScaleTransform(
            ClientSize.Width / (float)_engine.FieldWidth,
            ClientSize.Height / (float)_engine.FieldHeight);

        DrawSky(g, _engine);

        foreach (PipePair pipe in _engine.Pipes)
        {
            pipe.Draw(g);
        }

        DrawGround(g, _engine);
        _engine.Bird.Draw(g);
        DrawScore(g, _engine);
        DrawOverlay(g, _engine);
    }

    private static void DrawSky(Graphics g, GameEngine engine)
    {
        var sky = new RectangleF(0, 0, engine.FieldWidth, engine.GroundTop);
        using var brush = new LinearGradientBrush(
            sky,
            Color.FromArgb(56, 189, 248),
            Color.FromArgb(186, 230, 253),
            LinearGradientMode.Vertical);
        g.FillRectangle(brush, sky);
    }

    private void DrawGround(Graphics g, GameEngine engine)
    {
        float height = engine.FieldHeight - engine.GroundTop;

        using var soil = new SolidBrush(Color.FromArgb(217, 180, 120));
        using var grass = new SolidBrush(Color.FromArgb(101, 163, 13));
        using var stripe = new SolidBrush(Color.FromArgb(77, 124, 15));

        g.FillRectangle(soil, 0, engine.GroundTop, engine.FieldWidth, height);
        g.FillRectangle(grass, 0, engine.GroundTop, engine.FieldWidth, 14);

        // Zemin çizgileri oyun hızıyla kayarak hareket hissi verir.
        const float stripeSpacing = 30f;
        if (engine.State == GameState.Running)
        {
            _groundScroll = (_groundScroll + engine.Speed) % stripeSpacing;
        }

        for (float x = -_groundScroll; x < engine.FieldWidth; x += stripeSpacing)
        {
            g.FillRectangle(stripe, x, engine.GroundTop + 4, 14, 6);
        }
    }

    private static void DrawScore(Graphics g, GameEngine engine)
    {
        using var font = new Font("Segoe UI", 34f, FontStyle.Bold, GraphicsUnit.Pixel);
        DrawCenteredText(g, engine.Score.ToString(), font, Color.White, engine.FieldWidth, 24);
    }

    private static void DrawOverlay(Graphics g, GameEngine engine)
    {
        if (engine.State == GameState.Running)
        {
            return;
        }

        using var dim = new SolidBrush(Color.FromArgb(120, 15, 23, 42));
        g.FillRectangle(dim, 0, 0, engine.FieldWidth, engine.FieldHeight);

        using var titleFont = new Font("Segoe UI", 44f, FontStyle.Bold, GraphicsUnit.Pixel);
        using var textFont = new Font("Segoe UI", 19f, FontStyle.Regular, GraphicsUnit.Pixel);

        float center = engine.GroundTop / 2f;
        if (engine.State == GameState.GameOver)
        {
            DrawCenteredText(g, "GAME OVER", titleFont, Color.FromArgb(254, 202, 202), engine.FieldWidth, center - 70);
            DrawCenteredText(g, $"Skor: {engine.Score}", textFont, Color.White, engine.FieldWidth, center);
            DrawCenteredText(g, "Yeniden başlamak için R veya Yeni Oyun", textFont, Color.White, engine.FieldWidth, center + 32);
        }
        else
        {
            DrawCenteredText(g, "HAZIR", titleFont, Color.White, engine.FieldWidth, center - 70);
            DrawCenteredText(g, "Başlamak için Boşluk veya Yukarı Ok", textFont, Color.White, engine.FieldWidth, center);
        }
    }

    private static void DrawCenteredText(Graphics g, string text, Font font, Color color, float width, float y)
    {
        SizeF size = g.MeasureString(text, font);
        float x = (width - size.Width) / 2f;

        using var shadow = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
        using var brush = new SolidBrush(color);
        g.DrawString(text, font, shadow, x + 2, y + 2);
        g.DrawString(text, font, brush, x, y);
    }
}
