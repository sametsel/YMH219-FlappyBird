using System.Drawing.Drawing2D;

namespace FlappyBird.Game;

/// <summary>Oyuncunun kontrol ettiği kuş: yer çekimi ile düşer, zıplama ile yükselir.</summary>
public sealed class Bird : GameObject
{
    private const float Gravity = 0.45f;      // Her adımda hıza eklenen ivme (piksel/adım²)
    private const float FlapVelocity = -7.5f; // Zıplama anında verilen yukarı yönlü hız
    private const float MaxFallSpeed = 10f;   // Düşüş hızı üst sınırı

    // Çarpışma kutusu görselden biraz küçük tutulur; böylece kenar sürtmeleri haksız ölüm saymaz.
    private const float HitboxInset = 3f;

    public float Velocity { get; private set; }

    /// <summary>Çarpışma kontrolünde kullanılan dikdörtgen.</summary>
    public RectangleF Hitbox => new(
        X + HitboxInset,
        Y + HitboxInset,
        Width - 2 * HitboxInset,
        Height - 2 * HitboxInset);

    public Bird(float x, float y) : base(x, y, 38, 28)
    {
    }

    public void Flap() => Velocity = FlapVelocity;

    public override void Update()
    {
        Velocity = Math.Min(Velocity + Gravity, MaxFallSpeed);
        Y += Velocity;
    }

    /// <summary>Kuşu tavanda tutar: tavan ölümcül değildir, yalnızca yükselmeyi durdurur.</summary>
    public void ClampToCeiling()
    {
        if (Y < 0)
        {
            Y = 0;
            Velocity = 0;
        }
    }

    /// <summary>Kuşu zemine oturtur (zemine çarpıldığında çağrılır).</summary>
    public void RestOn(float groundTop)
    {
        Y = groundTop - Height;
        Velocity = 0;
    }

    public override void Draw(Graphics graphics)
    {
        GraphicsState saved = graphics.Save();

        // Kuş, hızına göre yukarı veya aşağı doğru eğilir.
        float angle = Math.Clamp(Velocity * 4f, -25f, 60f);
        graphics.TranslateTransform(X + Width / 2, Y + Height / 2);
        graphics.RotateTransform(angle);

        float left = -Width / 2;
        float top = -Height / 2;

        using var body = new SolidBrush(Color.FromArgb(250, 204, 21));
        using var wing = new SolidBrush(Color.FromArgb(253, 230, 138));
        using var beak = new SolidBrush(Color.FromArgb(234, 88, 12));
        using var outline = new Pen(Color.FromArgb(120, 53, 15), 2f);

        graphics.FillEllipse(body, left, top, Width, Height);
        graphics.DrawEllipse(outline, left, top, Width, Height);

        graphics.FillEllipse(wing, left + 4, top + 12, 16, 10);
        graphics.DrawEllipse(outline, left + 4, top + 12, 16, 10);

        graphics.FillEllipse(Brushes.White, left + 22, top + 3, 11, 11);
        graphics.FillEllipse(Brushes.Black, left + 27, top + 6, 5, 5);

        PointF[] beakPoints =
        {
            new(left + Width - 4, top + 12),
            new(left + Width + 8, top + 16),
            new(left + Width - 4, top + 21),
        };
        graphics.FillPolygon(beak, beakPoints);
        graphics.DrawPolygon(outline, beakPoints);

        graphics.Restore(saved);
    }
}
