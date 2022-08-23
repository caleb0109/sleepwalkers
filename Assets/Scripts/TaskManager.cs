using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public List<TextAsset> taskFiles;
    public Text toDoWidget;
    public Text toDoApp;

    private int currAct;

    private string currTask;
    private int taskIndex;
    private List<string> taskList;
    private List<string> requirements;
    private List<string> clues;

    // Start is called before the first frame update
    void Start()
    {
        taskList = new List<string>();
        requirements = new List<string>();
        currAct = 0; // when loading a game, get the task from that specific task;

        LoadTaskFile();
        SetTask();
    }

    private void LoadTaskFile()
    {
        List<string> tasks = new List<string>(taskFiles[currAct].text.Split('\n')); // holds the tasks split from the file

        for(int i = 0; i < tasks.Count; i++) 
        {
            string task = tasks[i]; // hold the task for check below

            // if it contains ":" there are req for the task
            if(task.Contains(":"))
            {
                List<string> tReq = new List<string>(task.Split(':'));
                requirements.AddRange(tReq[1].Split('?')); // adds requirements to the list
                task = tReq[0]; // task name
            }

            taskList.Add(task); // add the task to the list
        }
    }

    // used when loading save file
    private void SetTask()
    {
        //currTask = ; // sets the currTask to the saved task

        currTask = taskList[0]; // temp until ^ is implemented

        // find the index of the task
        for (int i = 0; i < taskList.Count; i++)
        {
            if (currTask == taskList[i])
            {
                taskIndex = i;
                break;
            }
        }

    }

    // updates the text on the to-do list widget and the app itself
    private void UpdateTaskView()
    {

    }

    // adds clues to the task (only in app view)
    private void AddClue()
    {

    }

    // moves to the next task in the list
    public void CompleteTask()
    {
        taskIndex++; // increment the task

        if (taskIndex < taskList.Count)
        {
            currTask = taskList[taskIndex];
            FindObjectOfType<NotificationManager>().NotifyTaskUpdate(currTask); // update next task
            UpdateTaskView();
            return; // exit out of method early
        }

        // if the tasks for this act is done, set everything to null/default
        currTask = "";
        taskIndex = 0;
    }

    // used for changing how the specific requirement is displayed
    public void CompleteSideTask()
    {

    }
}
