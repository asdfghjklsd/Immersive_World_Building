using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class LinearTaskManager : MonoBehaviour
{
    [Header("Level Sequence")]
    public List<TaskBaseClass> taskSequence;
    
    [Header("Completion Events")]
    [Tooltip("Fires when the entire task list is completed.")]
    public UnityEvent onSequenceComplete; 

    private int currentTaskIndex = 0;

    void Start()
    {
        foreach (TaskBaseClass task in taskSequence)
        {
            task.isActiveTask = false;
            task.onTaskCompleted.AddListener(AdvanceToNextTask);
        }

        if (taskSequence.Count > 0)
        {
            taskSequence[0].isActiveTask = true;
        }
    }

    private void AdvanceToNextTask()
    {
        currentTaskIndex++;

        if (currentTaskIndex < taskSequence.Count)
        {
            TaskBaseClass nextTask = taskSequence[currentTaskIndex];
            nextTask.isActiveTask = true;
        }
        else
        {
            Debug.Log("All tasks complete. Triggering save prompt.");
            // Tell the replay system to stop and save
            onSequenceComplete.Invoke(); 
        }
    }
}