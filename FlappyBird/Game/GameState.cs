namespace FlappyBird.Game;

public enum GameState
{
    /// <summary>Oyun hazır, oyuncunun ilk tuşa basması bekleniyor.</summary>
    Ready,

    /// <summary>Oyun akıyor.</summary>
    Running,

    /// <summary>Kuş çarptı, yeniden başlatılması bekleniyor.</summary>
    GameOver,
}
