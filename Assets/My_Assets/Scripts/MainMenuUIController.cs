using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class MainMenuUIController : MonoBehaviour
{
    [System.Serializable]
    public struct LevelEntry
    {
        public string displayName;
        public string sceneBuildName;
    }

    [Header("Empty State")]
    public GameObject emptyStateText; // Drag the EmptyStateText GameObject here

    [Header("Level Configuration")]
    public List<LevelEntry> availableLevels = new List<LevelEntry>();

    [Header("Panels")]
    public GameObject mainHomePanel;
    public GameObject levelListPanel;
    public GameObject reportListPanel;

    [Header("Dynamic List Containers")]
    public Transform levelButtonContainer;  // GameObject with VerticalLayoutGroup
    public Transform reportButtonContainer; // GameObject with VerticalLayoutGroup
    public GameObject listButtonPrefab;     // Prefab with Button & TMP_Text

    void Start()
    {
        ShowHome();
    }

    public void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ShowHome()
    {
        mainHomePanel.SetActive(true);
        levelListPanel.SetActive(false);
        reportListPanel.SetActive(false);
    }

    // Called by "Start Evaluation" button
    public void OpenLevelSelect()
    {
        mainHomePanel.SetActive(false);
        levelListPanel.SetActive(true);

        // Clear existing dynamic buttons
        foreach (Transform child in levelButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Spawn a button for each level configured in the inspector
        foreach (var level in availableLevels)
        {
            GameObject btnObj = Instantiate(listButtonPrefab, levelButtonContainer);
            btnObj.GetComponentInChildren<TMP_Text>().text = level.displayName;

            string sceneToLoad = level.sceneBuildName;
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                AppSessionManager.Instance.LaunchLevelFlow(sceneToLoad);
            });
        }
    }

    // Called by "View Ergonomics Reports" button
    public void OpenReportSelect()
    {
        mainHomePanel.SetActive(false);
        reportListPanel.SetActive(true);

        // Clear existing dynamic buttons
        foreach (Transform child in reportButtonContainer)
        {
            Destroy(child.gameObject);
        }

        string logsDirectory = Path.Combine(Application.persistentDataPath, "ErgoLogs");

        if (!Directory.Exists(logsDirectory))
        {
            Directory.CreateDirectory(logsDirectory);
        }

        string[] filePaths = Directory.GetFiles(logsDirectory, "*.json");

        // Toggle the empty state message based on file count
        if (filePaths.Length == 0)
        {
            emptyStateText.SetActive(true);
            return; // Stop execution, nothing to spawn
        }
        
        emptyStateText.SetActive(false);

        // Spawn a button for each found log
        foreach (string fullPath in filePaths)
        {
            string fileName = Path.GetFileNameWithoutExtension(fullPath);

            GameObject btnObj = Instantiate(listButtonPrefab, reportButtonContainer);
            btnObj.GetComponentInChildren<TMP_Text>().text = fileName;

            string selectedFile = fullPath;
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                AppSessionManager.Instance.LaunchReplayFlow(selectedFile);
            });
        }
    }
}