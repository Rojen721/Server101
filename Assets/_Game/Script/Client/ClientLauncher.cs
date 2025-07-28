using System.Threading.Tasks;
using Fusion;
using UnityEngine;

/// <summary>
/// ClientLauncher sınıfı, client başlatıldığında ilgili platforma uygun server uygulamasını çalıştırmak için kullanılır.
/// Bu script, CLNT_StarterScene içindeki boş bir GameObject'e eklenmelidir.
/// </summary>
public class ClientLauncher : MonoBehaviour
{
    [Header("Platforma göre server yolu")]

    /// <summary>
    /// macOS üzerinde server uygulamasının çalıştırılacağı dosya yolu.
    /// </summary>
     public string macAppPath = "/Users/rojenonat/Server101/ServerTest/MyServer.app";

    /// <summary>
    /// Windows üzerinde server uygulamasının çalıştırılacağı dosya yolu.
    /// </summary>
    [SerializeField] private string windowsExePath = "C:\\Server101\\ServerTest\\MyServer.exe";

    
    private async void Start()
    {
        // Birkaç saniye bekleyelim ki server ayağa kalksın
        await Task.Delay(2000);

        var runner = gameObject.AddComponent<NetworkRunner>();
        runner.ProvideInput = false;

        runner.AddCallbacks(gameObject.AddComponent<DummyClientCallbacks>() ); // INetworkRunnerCallbacks implementasyonu

        FusionGameStarter.StartGame(
            runner,
            "Lobby",
            GameMode.Client,
            SceneRef.None,
            runner.GetComponent<NetworkSceneManagerDefault>()
        );
    }
    
    private void Awake()
    {
        // Çalıştırılan platforma uygun server uygulamasının yolunu belirle
        string serverPath = GetPlatformPath();

        // Server başlatma bilgilerini logla, böylece hangi yol ve parametrelerle başlatıldığı görülebilir
        Debug.Log($"[CLIENT LAUNCHER] Server başlatılıyor: {serverPath} -gameMode lobby -sessionName Lobby");
        // Belirlenen server yolunu kullanarak server uygulamasını başlatır.
        // "lobby " parametresi oyun modunu, "Lobby" ise session adını belirtir.
        CrossPlatformServerLauncher.LaunchServer(serverPath, "lobby", "Lobby");
        
    }

    /// <summary>
    /// Çalışılan platforma göre server uygulamasının dosya yolunu döner.
    /// macOS için macAppPath, Windows için windowsExePath kullanılır.
    /// Desteklenmeyen platformlarda hata loglanır ve boş string döner.
    /// </summary>
    /// <returns>Platforma uygun server uygulaması dosya yolu</returns>
    private string GetPlatformPath()
    {
#if UNITY_STANDALONE_OSX
        return macAppPath;
#elif UNITY_STANDALONE_WIN
        return windowsExePath;
#else
        Debug.LogError("Desteklenmeyen platform!");
        return string.Empty;
#endif
    }
}