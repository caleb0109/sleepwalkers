using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    private Dictionary<string, string> tutorials;
    [HideInInspector] public List<string> tutorialNames;
    public Dictionary<string, string> completedTutorials;
    public bool inBattle;

    private List<bool> phoneTutorialCompleted;
    private NotificationManager notifManager;
    private Battle battleManager;
    private List<GameObject> battleTutorial;

    // Start is called before the first frame update
    void Start()
    {
        if (!inBattle)
        {
            notifManager = FindObjectOfType<NotificationManager>();

            tutorials = new Dictionary<string, string>();
            tutorialNames = new List<string>();
            completedTutorials = new Dictionary<string, string>();
            phoneTutorialCompleted = new List<bool>();

            // movement
            tutorials.Add("Movement", "Use 'WASD' to move");
            // interaction
            tutorials.Add("Interaction", "Use 'E' to interact with glowing objects");
            // opening/closing phone
            tutorials.Add("Open/Close Phone", "Use 'TAB' to open or close your phone");

            // add the tutorial names to the list
            foreach (string s in tutorials.Keys)
            {
                tutorialNames.Add(s);
            }
            // add the phone tutorial status OR  add the GameObjects of the battle tutorials
            for (int i = 0; i < this.transform.childCount; i++)
            {
                phoneTutorialCompleted.Add(false);
            }
        }
        else
        {
            battleManager = FindObjectOfType<Battle>();
            battleTutorial = new List<GameObject>();

            for (int i = 0; i < this.transform.childCount; i++)
            {
                battleTutorial.Add(this.transform.GetChild(i).gameObject);
            }
        }
        
    }

    void Update()
    {
        if (inBattle)
        {
            CheckBattleState();
        }
    }

    // used find the tutorial in the the dictionary
    public void StartTutorial(string name)
    {
        // if the dictionary contains the key, show the tutorial and added to completed list while removing it from the lists themselves
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

    // check what the current state of the turns
    private void CheckBattleState()
    {
        if (battleManager.state != BattleState.PlayerTurn)
        {
            // if the tutorial is active in the hierarchy, set inactive and set the next tutorial active
            if (battleTutorial[0].activeInHierarchy)
            {
                battleTutorial[0].SetActive(false);
                battleTutorial[1].SetActive(true);
            }
        }
        else if (battleManager.state != BattleState.EnemyTurn)
        {
            if (battleTutorial[1].activeInHierarchy)
            {
                battleTutorial[1].SetActive(false);
            }
        }
    }

    public void PhoneTutorial()
    {
        Transform tManager = this.transform;

        // go through the children and take turns activating them/completing them
        for (int i = 0; i < tManager.childCount; i++)
        {
            GameObject child = tManager.GetChild(i).gameObject;

            if (!child.activeInHierarchy && !phoneTutorialCompleted[i])
            {
                child.SetActive(true);
                phoneTutorialCompleted[i] = true;
                TutorialDuration(); 
                break;
            }
        }
    }

    // used to iterate through the phone tutorials temporarily
    IEnumerator TutorialDuration()
    {
        float duration = 3f;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            
            if (duration < 0f)
            {
                PhoneTutorial();
            }

            yield return null;
        }

    }
}
