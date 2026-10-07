using UnityEngine;

// Notice it inherits from AssemblyTask instead of MonoBehaviour
public class ButtonPressTask : TaskBaseClass
{
    // You will link this public method directly to your UI Button's OnClick() event
    public void OnButtonPressed()
    {
        // Attempt to complete the task. 
        // The base class will silently block this if Task 1 isn't done yet.
        MarkTaskComplete(); 
    }
}