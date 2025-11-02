using System;
using UnityEngine;

public static class AndroidLogManager
{
    private static AndroidJavaObject _logManagerInstance;
    private static AndroidJavaObject _currentActivity;
    private static AndroidJavaClass _unityPlayer;

    private static void Init()
    {
        if (_logManagerInstance != null) 
            return;

        try
        {
            _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _currentActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.logPlugin.LogManager");
            _logManagerInstance = pluginClass.CallStatic<AndroidJavaObject>("create", _currentActivity);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to init log manager: " + e.Message);
        }
    }

    public static void SendLog(string message)
    {
        Init();
        _logManagerInstance?.Call("sendLog", message);
    }

    public static string GetLogs()
    {
        Init();
        return _logManagerInstance?.Call<string>("getLogs") ?? "Plugin not initialized. Could not get logs";
    }

    public static void ClearLogs()
    {
        Init();
        _logManagerInstance?.Call("clearLogs");
    }
}
