using UnityEngine;
using UnityEngine.SceneManagement;

public class AppSessionManager : MonoBehaviour
{
    public static AppSessionManager Instance;

    [Header("Session Payload")]
    public string targetLevelSceneName;
    public string selectedReportFilePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when a user selects a level
    public void LaunchLevelFlow(string levelSceneName)
    {
        targetLevelSceneName = levelSceneName;
        // Route to the Transition Scene first so calibration can happen
        SceneManager.LoadScene(targetLevelSceneName);
    }

    // Called when a user selects a saved report
    public void LaunchReplayFlow(string jsonPath)
    {
        selectedReportFilePath = jsonPath;
        // Directly load the Replay Scene; no calibration needed for replay
        SceneManager.LoadScene("ReplayScene");
    }

    // Called after calibration in Transition Scene finishes
    public void LoadTargetLevel(string levelSceneName)
    
    {
        if (!string.IsNullOrEmpty(targetLevelSceneName))
        {
            SceneManager.LoadScene(targetLevelSceneName);
        }
        else
        {
            Debug.LogError("No target level specified!");
        }
    }
}