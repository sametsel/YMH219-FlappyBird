namespace FlappyBird.Game;

/// <summary>Bir zorluk seviyesinin adı ve oyuna etkisi olan sayısal ayarları.</summary>
public sealed record DifficultyLevel(string Name, float BaseSpeed, float GapSize)
{
    public static readonly DifficultyLevel Easy = new("Kolay", 2.5f, 195f);
    public static readonly DifficultyLevel Normal = new("Normal", 3f, 170f);
    public static readonly DifficultyLevel Hard = new("Zor", 3.5f, 145f);

    public static IReadOnlyList<DifficultyLevel> All { get; } = new[] { Easy, Normal, Hard };

    // ComboBox öğeleri ekranda ToString() sonucu ile gösterilir.
    public override string ToString() => Name;
}
