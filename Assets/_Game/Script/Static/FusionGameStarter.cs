using Fusion;
using UnityEngine;

public static class FusionGameStarter
{
    /// <summary>
    /// Game başlatmak için kullanılabilecek statik method.
    /// Runner dışarıdan alınır.
    /// </summary>
    public static async void StartGame(NetworkRunner runner, string sessionName, GameMode gameMode, SceneRef sceneRef, INetworkSceneManager sceneManager)
    {
        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode     = gameMode,
            SessionName  = sessionName,
            Scene        = sceneRef,
            SceneManager = sceneManager
        });

        if (result.Ok)
            Debug.Log($"[FusionGameStarter] Session '{sessionName}' başlatıldı.");
        else
            Debug.LogError($"[FusionGameStarter] Session başlatılamadı: {result.ShutdownReason} , GameMode:{gameMode},SessionName:{sessionName} , Runner: {runner} , SceneManager:{sceneManager}");
    }
}