using System.Globalization;

namespace FlappyBird.Game;

/// <summary>En yüksek skoru kullanıcının yerel uygulama klasöründeki bir metin dosyasında saklar.</summary>
public sealed class HighScoreStore
{
    private readonly string _filePath;

    public HighScoreStore()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "YMH219-FlappyBird",
            "highscore.txt"))
    {
    }

    public HighScoreStore(string filePath)
    {
        _filePath = filePath;
    }

    /// <summary>Kayıtlı skoru okur. Dosya yoksa veya bozuksa oyun 0 ile devam eder.</summary>
    public int Load()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return 0;
            }

            string content = File.ReadAllText(_filePath).Trim();
            return int.TryParse(content, NumberStyles.None, CultureInfo.InvariantCulture, out int score)
                ? score
                : 0;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return 0;
        }
    }

    /// <summary>Skoru kaydeder. Yazılamazsa false döner; oyun bu yüzden kesilmez.</summary>
    public bool Save(int score)
    {
        try
        {
            string? directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(_filePath, score.ToString(CultureInfo.InvariantCulture));
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }
}
