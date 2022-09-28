using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private Dictionary<string, string> tutorials;
    [HideInInspector] public List<string> tutorialNames;
    public Dictionary<string, string> completedTutorials;

    // Start is called before the first frame update
    void Start()
    {
        tutorials = new Dictionary<string, string>();
        tutorialNames = new List<string>();
        completedTutorials = new Dictionary<string, string>();

        // movement
        tutorials.Add("Movement", "Use 'WASD' to move");
        // interaction
        tutorials.Add("Interaction", "Use 'E' to interact with glowing objects");
        // opening/closing phone
        tutorials.Add("Open/Close Phone", "Use 'TAB' to open or close your phone");

        foreach(string s in tutorials.Keys)
        {
            tutorialNames.Add(s);
        }
    }

    // used find the tutorial in the the dictionary
    public void StartTutorial(string name)
    {
        // if the dictionary contains the key, show the tutorial and added to completed list
        if (tutorials.ContainsKey(name))
        {
            FindObjectOfType<NotificationManager>().ShowTutorial(name, tutorials[name]);
            completedTutorials.Add(name, tutorials[name]);
            tutorials.Remove(name);
            tutorialNames.Remove(name);
        }
    }

    // check the conditions for proceeding to the next tutorial
    public void CheckTutorialCondition(string name)
    {
        if (!tutorials.ContainsKey(name))
        {
            StartTutorial(tutorialNames[0]);
        }
    }

    // used to set the gameobject inactive after its condition's been met and activates the next tutorial text
    public IEnumerator TutorialDuration(string name)
    {
        float duration = 3f;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            
            if (duration < 0f)
            {
                StopAllCoroutines();
                StartCoroutine(FindObjectOfType<NotificationManager>().NotificationTimer());
            }

            yield return null;
        }

    }
}
