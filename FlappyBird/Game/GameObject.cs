namespace FlappyBird.Game;

/// <summary>
/// Ekranda konumu olan, güncellenen ve çizilen tüm oyun nesnelerinin ortak taban sınıfı.
/// Kuş ve engel bu sınıftan türer; her biri kendi hareketini ve çizimini tanımlar.
/// </summary>
public abstract class GameObject
{
    public float X { get; protected set; }
    public float Y { get; protected set; }
    public float Width { get; }
    public float Height { get; }

    public float Right => X + Width;

    protected GameObject(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>Nesneyi bir oyun adımı (timer tick) kadar ilerletir.</summary>
    public abstract void Update();

    /// <summary>Nesneyi verilen grafik yüzeyine çizer.</summary>
    public abstract void Draw(Graphics graphics);
}
