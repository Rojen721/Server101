using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;


public static class CrossPlatformServerLauncher
{
    /// <summary>
    /// macOS ve Windows için binary yolunu döner.
    /// Açıklama: macOS'ta doğrudan çalıştırılabilir dosya verilir, .app dizinine girilmez.
    /// </summary>
    /// <param name="basePath">macOS için doğrudan binary, Windows için .exe yolunu ver</param>
    /// <param name="gameMode">GameMode argümanı</param>
    /// <param name="sessionName">SessionName argümanı</param>
    public static void LaunchServer(string basePath, string scene, string sessionName)
    {
        Debug.Log(basePath);
        string binaryPath = GetPlatformBinaryPath(basePath);

        if (!File.Exists(binaryPath))
        {
            Debug.LogError($"Server binary bulunamadı: {binaryPath}");
            return;
        }

        string args = $"-gameMode {scene} -sessionName {sessionName}";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = binaryPath,
            Arguments = args,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            Process.Start(startInfo);
            Debug.Log($"[Launcher] Server başlatıldı → {binaryPath} {args}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[Launcher] Server başlatılamadı: {ex.Message}");
        }
    }

    /// <summary>
    /// macOS ve Windows için binary yolunu döner.
    /// Açıklama: macOS'ta doğrudan çalıştırılabilir dosya verilir, .app dizinine girilmez.
    /// </summary>
    private static string GetPlatformBinaryPath(string basePath)
    {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // macOS'ta doğrudan çalıştırılabilir binary verilir (örneğin: /Users/xyz/Server101)
        return basePath;
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        return basePath; // Windows için zaten doğrudan .exe olmalı
#else
        throw new System.PlatformNotSupportedException("Bu platform desteklenmiyor.");
#endif
    }
}