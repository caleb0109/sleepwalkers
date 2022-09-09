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
    private List<string> reqCompleted;
    private List<string> clues; // used to store the clues
    private int cluesGiven; // used to keep track of number of clues given to player
    private bool appOnly;

    // Start is called before the first frame update
    void Start()
    {
        taskList = new List<string>();
        requirements = new List<string>();
        reqCompleted = new List<string>();
        clues = new List<string>();
        cluesGiven = 0;
        currAct = 0; // when loading a game, get the task from that specific task;
        appOnly = false;

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

        UpdateTaskView("main");
    }

    // updates the text on the to-do list widget and the app itself
    private void UpdateTaskView(string type)
    {         
        if (!appOnly) // if need to add side tasks and clues
        {
            toDoApp.text = currTask; // adds the main task to the app view
            toDoWidget.text = currTask; // adds the main task to the widget view
        }
        else // addes the clues and side tasks to the app view
        {
            switch(type)
            {
                case "clue":
                    break;

                case "side":
                    DisplaySideTask(requirements);
                    DisplaySideTask(reqCompleted);
                    break;
            }
        }
    }

    // fix later to prevent having multiples of the same text shown
    private void DisplaySideTask(List<string> reqs)
    {
        foreach (string r in reqs)
        {
            toDoApp.text += r; // display the requirements
        }
    }

    // adds clues to the task (only in app view)
    private void AddClue()
    {
        appOnly = true;
        cluesGiven++;
        UpdateTaskView("clue");
    }

    // moves to the next task in the list
    public void CompleteTask()
    {
        taskIndex++; // increment the task

        if (taskIndex < taskList.Count)
        {
            appOnly = false;
            currTask = taskList[taskIndex];
            FindObjectOfType<NotificationManager>().NotifyTaskUpdate(currTask); // update next task
            UpdateTaskView("main");
            return; // exit out of method early
        }

        // if the tasks for this act is done, set everything to null/default
        currTask = "";
        taskIndex = 0;
    }

    // used for changing how the specific requirement is displayed
    public void CompleteSideTask(string sideName)
    {
        appOnly = true;

        for (int i = 0; i < requirements.Count; i++)
        {
            if (requirements[i] == sideName)
            {
                reqCompleted.Add(requirements[i]);
                requirements.RemoveAt(i);
            }
        }

        UpdateTaskView("side");
    }
}
