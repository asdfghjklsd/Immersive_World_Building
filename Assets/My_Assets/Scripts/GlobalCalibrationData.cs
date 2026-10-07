using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GlobalCalibrationData : MonoBehaviour
{
    public static GlobalCalibrationData Instance;
    
    public float GlobalYawOffset { get; private set; }
    public Vector3 SavedHipLocalPosition { get; private set; }
    public bool IsCalibrated { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ExecuteCalibration(Transform cameraTransform, Transform currentHipTransform, string nextSceneName)
    {
        // 1. Recenter hardware tracking
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
        {
            subsystems[0].TryRecenter();
        }

        // 2. Record offsets and exact neutral hip location
        GlobalYawOffset = 0f - cameraTransform.eulerAngles.y;
        SavedHipLocalPosition = currentHipTransform.localPosition;
        IsCalibrated = true;

        // 3. Move to the Main Menu
        SceneManager.LoadScene(nextSceneName);
    }
}