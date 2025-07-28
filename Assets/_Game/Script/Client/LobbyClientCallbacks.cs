using System;
using Fusion;
using UnityEngine;
using System.Collections.Generic;
using Fusion.Sockets;

/// <summary>
/// Client tarafında Fusion olaylarını yakalayan callback sınıfı.
/// Bu script lobi sahnesine eklenir ve UI, oyuncu bağlantısı gibi şeyleri yönetir.
/// </summary>
public class LobbyClientCallbacks : MonoBehaviour, INetworkRunnerCallbacks
{
    /// <summary>
    /// Sunucuya başarıyla bağlandıysa tetiklenir.
    /// </summary>
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("[CLIENT] Sunucuya bağlandı.");
    }

    /// <summary>
    /// Sunucudan bağlantı koptuğunda tetiklenir.
    /// Örneğin server kapanırsa veya internet giderse.
    /// </summary>
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.LogWarning("[CLIENT] Sunucu ile bağlantı koptu.");
    }

    /// <summary>
    /// Session kapanırsa (örneğin StartGame başarısız olursa veya bağlantı hatası olursa) tetiklenir.
    /// </summary>
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.LogWarning($"[CLIENT] Runner durduruldu → Sebep: {shutdownReason}");
    }

    /// <summary>
    /// Sunucudan bağlantı koptuğunda tetiklenir ve bağlantı kopma sebebini bildirir.
    /// Client açısından, bağlantı neden koptuğunu analiz etmek için kullanılabilir.
    /// </summary>
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.LogWarning($"[CLIENT] Sunucu ile bağlantı koptu. Sebep: {reason}");
    }

    /// <summary>
    /// Session listesi her güncellendiğinde tetiklenir (örneğin başka biri yeni bir oda kurduğunda).
    /// Buradan UI listesi güncellenebilir.
    /// </summary>
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log($"[CLIENT] Yeni session listesi alındı → {sessionList.Count} adet oda bulundu.");

        // Buraya UI'daki oda listeni güncelleyen kodu yazabilirsin.
    }

    /// <summary>
    /// Bir network objesi, bir oyuncunun AOI (görüş alanı) dışına çıktığında tetiklenir.
    /// Client açısından, objenin artık oyuncu için görünmez olacağı anlamına gelir.
    /// </summary>
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        // Gerekirse objenin oyuncudan gizlenmesi gibi işlemler yapılabilir.
    }


    /// <summary>
    /// Bir network objesi, bir oyuncunun AOI (görüş alanı) içine girdiğinde tetiklenir.
    /// Client açısından, objenin oyuncu için artık görünür olacağı anlamına gelir.
    /// </summary>
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        // Gerekirse objenin oyuncuya gösterilmesi gibi işlemler yapılabilir.
    }


    /// <summary>
    /// Bu client'ın bağlı olduğu session'a başka bir oyuncu katıldığında tetiklenir.
    /// (örn. aynı odaya başka biri girdiğinde)
    /// </summary>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"[CLIENT] Yeni oyuncu katıldı: {player}");
    }

    /// <summary>
    /// Bu client'ın bağlı olduğu session'dan bir oyuncu ayrıldığında tetiklenir.
    /// </summary>
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"[CLIENT] Oyuncu ayrıldı: {player}");
    }

    // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

    // Aşağıdaki metodlar genelde client tarafında kullanılmaz ama boş implementasyon yapılmalı.

    /// <summary>
    /// Güvenilir veri transferi sırasında ilerleme bilgisi sağlandığında tetiklenir.
    /// Client açısından, büyük veri transferlerinde ilerlemeyi takip etmek için kullanılabilir.
    /// </summary>
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        // Transfer ilerlemesini izlemek için kullanılabilir.
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) {}
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {}
    /// <summary>
    /// Sunucuya bağlantı kurulamazsa tetiklenir.
    /// Client açısından, bağlanma denemesi başarısız olduğunda hata yönetimi için kullanılır.
    /// </summary>
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.LogError($"[CLIENT] Sunucuya bağlanılamadı. Sebep: {reason}");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {}
    /// <summary>
    /// Güvenilir veri transferi tamamlandığında tetiklenir (anahtar ile birlikte).
    /// Client açısından, özel veri paketleri alındığında işlemek için kullanılabilir.
    /// </summary>
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        // Alınan veriyi işlemek için kullanılabilir.
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {}
    /// <summary>
    /// Güvenilir veri transferi tamamlandığında tetiklenir (anahtar olmadan).
    /// Client açısından, özel veri paketleri alındığında işlemek için kullanılabilir.
    /// </summary>
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data)
    {
        // Alınan veriyi işlemek için kullanılabilir.
    }
    public void OnSceneLoadDone(NetworkRunner runner) {}
    public void OnSceneLoadStart(NetworkRunner runner) {}
}