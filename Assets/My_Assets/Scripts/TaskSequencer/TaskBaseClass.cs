using UnityEngine;
using UnityEngine.Events;

public abstract class TaskBaseClass : MonoBehaviour
{
    public string taskName;
    public bool isComplete = false;
    
    // The TaskManager turns this on when it is this task's turn
    public bool isActiveTask = false; 
    
    public UnityEvent onTaskCompleted;

    // Derived scripts will call this function when their unique condition is met
    protected void MarkTaskComplete()
    {
        // Reject the completion if it's not this task's turn yet
        if (!isActiveTask || isComplete) 
        {
            Debug.LogWarning($"{taskName} was attempted, but it is not the active task.");
            return;
        }

        isComplete = true;
        isActiveTask = false;
        
        // Announce that this task is done so the Manager can move to the next one
        onTaskCompleted.Invoke();
    }
}