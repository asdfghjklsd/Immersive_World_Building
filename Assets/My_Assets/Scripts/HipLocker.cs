using UnityEngine;

public class HipLocker : MonoBehaviour
{
    [Header("Bone References")]
    [Tooltip("Drag the actual Hip/Pelvis bone of your 3D model here.")]
    public Transform hipBone;

    void LateUpdate()
    {
        // Only lock the hips if calibration data exists
        if (hipBone != null && GlobalCalibrationData.Instance != null && GlobalCalibrationData.Instance.IsCalibrated)
        {
            // Override the Meta SDK to lock the hips at the captured calibration coordinate
            hipBone.localPosition = GlobalCalibrationData.Instance.SavedHipLocalPosition;
            
            // Force the hips to always face exactly forward in the local tracking space
            hipBone.localRotation = Quaternion.identity; 
        }
    }
}