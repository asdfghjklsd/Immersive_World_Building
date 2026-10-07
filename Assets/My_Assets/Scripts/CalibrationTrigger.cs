using UnityEngine;

public class CalibrationTrigger : MonoBehaviour
{
    [Header("Tracking References")]
    [Tooltip("Drag the Main Camera here")]
    public Transform mainCamera;
    
    [Tooltip("Drag the Avatar's Hip/Pelvis bone here")]
    public Transform avatarHipBone;

    [Header("Scene Transition")]
    public string nextSceneName = "MainMenu";

    // Link this to your UI Button
    public void OnCalibrateClicked()
    {
        if (GlobalCalibrationData.Instance != null)
        {
            GlobalCalibrationData.Instance.ExecuteCalibration(mainCamera, avatarHipBone, nextSceneName);
        }
        else
        {
            Debug.LogError("GlobalCalibrationData manager is missing from the scene!");
        }
    }
}