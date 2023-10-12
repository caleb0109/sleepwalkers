using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    // used for each tutorial to see if there's any special condition to activate it
    public enum TutorialCondition
    {
        None,
        Trigger,
        Item
    }

    #region Variables
    private bool started;
    private bool completed;

    public TutorialCondition condition; // set in the inspector
    public GameObject reqItem;
    #endregion

    public bool Completed { get { return completed; } }

    // Start is called before the first frame update
    void Start()
    {
        // initialize variables
        started = false;
        completed = false;
    }

    void Update()
    {

    }

    public void StartTutorial()
    {
        if (!started)
        {
            started = true;
            this.gameObject.SetActive(true);

            switch (condition)
            {
                case TutorialCondition.Item:
                    FindObjectOfType<Inventory>().CheckInventory(reqItem);
                    break;

                case TutorialCondition.Trigger:
                    break;

                default: // same as TutorialCondition.None
                    break;
            }
        }
    }

    public void CompleteTutorial()
    {
        completed = true;
        this.gameObject.SetActive(false);
    }
   
}
