using UnityEngine;

public class CalibrationController : MonoBehaviour
{
    // This function calls the persistent manager that survived the scene load
    public void FinishCalibration()
    {
        if (AppSessionManager.Instance != null)
        {
            AppSessionManager.Instance.LoadTargetLevel();
        }
        else
        {
            Debug.LogError("AppSessionManager is missing! Make sure you started from the Main Menu.");
        }
    }
}