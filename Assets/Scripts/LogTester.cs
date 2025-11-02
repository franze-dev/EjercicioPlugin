using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogTester : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private Button _showLogsButton;
    [SerializeField] private Button _clearLogsButton;
    [SerializeField] private Button _testLogButton;
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _refreshButton;
    [SerializeField] private int _characterLimit = 500;

    private string currentMessage = "Tap buttons to test logs";

    void Start()
    {
        _showLogsButton.onClick.AddListener(ShowLogs);
        _clearLogsButton.onClick.AddListener(ClearLogs);
        _testLogButton.onClick.AddListener(TestLog);
        _helpButton.onClick.AddListener(ShowHelp);
        _refreshButton.onClick.AddListener(RefreshStatus);

        UpdateStatus("Ready! Tap Help for info");
        TestPluginFunctions();
    }

    void Update()
    {
        if (_statusText != null)
            _statusText.text = currentMessage;
    }

    void UpdateStatus(string message)
    {
        currentMessage = message;
        Debug.Log($"UI: {message}");
    }

    void TestPluginFunctions()
    {
        UpdateStatus("Testing plugin...");
        string logs = AndroidLogManager.GetLogs();
        UpdateStatus($"System Ready! Logs: {logs.Length} chars");
    }
    public void ShowLogs()
    {
        UpdateStatus("Loading logs...");
        string logs = AndroidLogManager.GetLogs();

        if (logs.Length > _characterLimit)
        {
            string preview = logs.Length > _characterLimit * 2 ? logs.Substring(0, _characterLimit * 2) + "..." : logs;
            UpdateStatus($"Logs: {logs.Length} chars\nPreview: {preview}");
        }
        else
        {
            UpdateStatus($"Logs: {logs}");
        }

        Debug.Log("=== ALL SAVED LOGS ===");
        Debug.Log(logs);
        Debug.Log("=== END LOGS ===");
    }

    public void ClearLogs()
    {
        UpdateStatus("Requesting log clearance...");
        AndroidLogManager.ClearLogs();
        UpdateStatus("Check device for confirmation dialog!");
    }

    public void TestLog()
    {
        Debug.Log($"Test log at time: {Time.time}");
        UpdateStatus($"Test log added at {Time.time:F2}");

        if (Time.time % 3f < 1f)
            Debug.LogWarning($"Warning test at {Time.time}");
        else if (Time.time % 3f < 2f)
            Debug.LogError($"Error test at {Time.time}");
    }

    public void ShowHelp()
    {
        string helpMessage = "Buttons:\n" +
                            "Show Logs - Display saved logs\n" +
                            "Clear Logs - Delete all logs\n" +
                            "Test Log - Add test message\n" +
                            "Help - Show this info\n" +
                            "Refresh - Update status";

        UpdateStatus(helpMessage);
        Debug.Log("=== BUTTON CONTROLS ===");
        Debug.Log("Show Logs - Display all saved logs");
        Debug.Log("Clear Logs - Delete logs (with Android confirmation)");
        Debug.Log("Test Log - Add test log message");
        Debug.Log("Help - Show help information");
        Debug.Log("Refresh - Refresh status display");
    }

    public void RefreshStatus()
    {
        string logs = AndroidLogManager.GetLogs();
        int lineCount = logs.Split('\n').Length;
        UpdateStatus($"Status: {lineCount} log lines\nTap buttons to test");
    }
}