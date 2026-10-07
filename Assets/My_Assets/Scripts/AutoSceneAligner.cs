using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;

public class AutoSceneAligner : MonoBehaviour
{
    [Header("Rig References")]
    public XROrigin xrOrigin;

    void Start()
    {
        StartCoroutine(AlignToPositiveZSequence());
    }

    private IEnumerator AlignToPositiveZSequence()
    {
        // 1. Wait a fraction of a second for the XR hardware to initialize
        yield return new WaitForSeconds(0.1f);

        // 2. Find the active XRInputSubsystem to recenter the headset
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);;
        
        if (subsystems.Count > 0)
        {
            // TryRecenter() acts exactly like the old Recenter(), untwisting the Meta Retargeter pelvis
            subsystems[0].TryRecenter();
        }
        else
        {
            Debug.LogWarning("No active XRInputSubsystem found. Cannot recenter tracking.");
        }

        // 3. Wait one frame for the recenter to physically apply to the Main Camera
        yield return null;

        // 4. Snap the XR Origin so the headset faces exactly Positive Z (0 degrees)
        if (xrOrigin != null && xrOrigin.Camera != null)
        {
            float currentYaw = xrOrigin.Camera.transform.eulerAngles.y;
            float yawOffset = 0f - currentYaw;

            // Rotate the entire XR rig around the player's physical head position
            xrOrigin.transform.RotateAround(xrOrigin.Camera.transform.position, Vector3.up, yawOffset);
        }
    }
}