using Fusion;
using UnityEngine;

/// <summary>
/// Server build başladığında otomatik çalışır.
/// Komut satırı parametresine göre ilgili sahneye geçer ve runner başlatır.
/// </summary>
public class ServerBootstrap : MonoBehaviour
{
    private async void Awake()
    {
        // Komut satırı argümanlarını oku
        string[] args = System.Environment.GetCommandLineArgs();

        string gameMode = null;
        string sessionName = null;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-gameMode" && i + 1 < args.Length)
                gameMode = args[i + 1];

            if (args[i] == "-sessionName" && i + 1 < args.Length)
                sessionName = args[i + 1];
        }

        Debug.Log($"[SERVER STARTER] Başlatılıyor → GameMode: {gameMode}, Session: {sessionName}");

        // Runner + SceneManager kur
        var runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = false;

        var sceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
        runner.AddCallbacks(gameObject.AddComponent<DummyServerCallbacks>());

        // GameMode'a göre sahne indexini al
        int sceneIndex = GetSceneIndexFromGameMode(gameMode);

        // Start server runner
        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Server,
            SessionName = sessionName,
            Scene = SceneRef.FromIndex(sceneIndex),
            SceneManager = sceneManager
        });

        if (result.Ok)
            Debug.Log($"[SERVER STARTER] Runner başlatıldı → SceneIndex: {sceneIndex}");
        else
            Debug.LogError($"[SERVER STARTER] Başlatma hatası: {result.ShutdownReason}");
    }

    private int GetSceneIndexFromGameMode(string gameMode)
    {
        switch (gameMode?.ToLowerInvariant())
        {
            case "lobby": return SceneDefs.LOBBY_SCENE;
            case "gameplay": return SceneDefs.GAMEPLAY_SCENE;
            default: return SceneDefs.SERVER_SCENE;
        }
    }
}