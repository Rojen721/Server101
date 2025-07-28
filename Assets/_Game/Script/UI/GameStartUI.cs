using Fusion;
using System;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField RoomNameInput;
    public Button StartGameButton;

    //[Header("Server Launch Ayarları")]
    private string ServerBinaryPath;

    private void Awake()
    {
        StartGameButton.onClick.AddListener(OnStartGameClicked);
        ServerBinaryPath = Application.platform switch
        {
            RuntimePlatform.OSXPlayer or RuntimePlatform.OSXEditor =>
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Server101/ServerTest/Server101"),

            RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor =>
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                             @"GitHub\Server101\ServerTest_Windows\Server101.exe"),

            _ => throw new System.NotSupportedException("Platform desteklenmiyor")
        };
    }

    private void OnStartGameClicked()
    {
        string roomName = RoomNameInput.text;
        if (string.IsNullOrWhiteSpace(roomName))
        {
            Debug.LogWarning("Lütfen geçerli bir oda ismi girin.");
            return;
        }
        StartClientAndServerAsync(roomName);
    }
    
    private async Task StartClientAndServerAsync(string roomName)
    {
        Debug.Log(ServerBinaryPath);
        // Server'ı başlat
        CrossPlatformServerLauncher.LaunchServer(ServerBinaryPath, "gameplay", roomName);

        var oldRunner = FindAnyObjectByType<NetworkRunner>();

        await oldRunner.Shutdown();
        
        await Task.Delay(5000); // server başlasın diye bekleme süresi

        var newRunnerGO = new GameObject("Client_Runner");
        var runner = newRunnerGO.AddComponent<NetworkRunner>();
        var sceneManager = newRunnerGO.AddComponent<NetworkSceneManagerDefault>();

        runner.ProvideInput = true;
        runner.AddCallbacks(newRunnerGO.AddComponent<DummyClientCallbacks>());
        
        FusionGameStarter.StartGame(runner,roomName,GameMode.Client,SceneRef.None,sceneManager);

        /*await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Client,
            SessionName = roomName,
            Scene = SceneRef.None,
            SceneManager = sceneManager
        });*/
    }

    /*private async System.Threading.Tasks.Task StartClientAndServerAsync(string roomName)
    {
        // Server'ı başlat
        //CrossPlatformServerLauncher.LaunchServer(ServerBinaryPath, "gameplay", roomName);

        await System.Threading.Tasks.Task.Delay(3000); // 3 saniyelik gecikme

        var runner = FindAnyObjectByType<NetworkRunner>();
        

        if (runner != null && runner.IsRunning)
        {
            await runner.Shutdown();
        }

        //var gameplayRunner = new GameObject("Gameplay_Runner");
       
        
        //NetworkRunner gameStarterRunner = gameplayRunner.AddComponent<NetworkRunner>();
        //var sceneManager = gameplayRunner.AddComponent<NetworkSceneManagerDefault>();
        
        //gameStarterRunner.ProvideInput = true;
        //gameStarterRunner.AddCallbacks(gameplayRunner.AddComponent<DummyClientCallbacks>());
      
        

        //FusionGameStarter.StartGame(gameStarterRunner, roomName, GameMode.Client, SceneRef.None, sceneManager);
        
        var runner = FindObjectOfType<NetworkRunner>();
        runner.ProvideInput = true;
        
        var newSceneManager = new GameObject("SceneMgr").AddComponent<NetworkSceneManagerDefault>();
        

        await runner.StartGame(
            new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SessionName = roomName,
                Scene = SceneRef.None,
                SceneManager = newSceneManager
            }

        );
        
        

    }*/
}