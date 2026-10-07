using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public struct BoneState 
{ 
    public Quaternion rotation; 
}

[System.Serializable]
public class ReplayFrame
{
    public float timestamp;
    public BoneState[] bones;
}

[System.Serializable]
public class ReplayData
{
    public string taskName;
    public System.Collections.Generic.List<ReplayFrame> frames = new System.Collections.Generic.List<ReplayFrame>();
}

public class ErgoReplayManager : MonoBehaviour
{
    [Header("Rig References")]
    public Transform[] ergoUpperBones;

    [Header("UI References")]
    public GameObject namePromptUI;

    private ReplayData currentReplay;
    private bool isRecording = false;
    private float recordingTimer = 0f;
    private float recordInterval = 0.1f;

    void Start()
    {
        // Start recording the entire level immediately
        StartRecording(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void StartRecording(string levelName)
    {
        currentReplay = new ReplayData { taskName = levelName };
        isRecording = true;
        recordingTimer = 0f;
        namePromptUI.SetActive(false);
    }

    void Update()
    {
        if (isRecording)
        {
            RecordFrame();
        }
    }

    private void RecordFrame()
    {
        recordingTimer += Time.deltaTime;
        if (recordingTimer >= recordInterval)
        {
            recordingTimer = 0f;
            ReplayFrame frame = new ReplayFrame { timestamp = Time.time, bones = new BoneState[ergoUpperBones.Length] };
            
            for (int i = 0; i < ergoUpperBones.Length; i++)
            {
                frame.bones[i] = new BoneState { rotation = ergoUpperBones[i].localRotation };
            }
            currentReplay.frames.Add(frame);
        }
    }

    // The LinearTaskManager will trigger this function
    public void StopRecordingAndShowPrompt()
    {
        isRecording = false;
        namePromptUI.SetActive(true); 
    }

    // Link this to the physical Save button on your UI
    public void SaveReplayAndExit(string fileNameInput)
    {
        string json = JsonUtility.ToJson(currentReplay);
        string path = Path.Combine(Application.persistentDataPath, fileNameInput + ".json");
        File.WriteAllText(path, json);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}