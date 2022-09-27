using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    public Dictionary<string, string> tutorials;

    // Start is called before the first frame update
    void Start()
    {
        tutorials = new Dictionary<string, string>();

        // movement
        tutorials.Add("Movement", "Use 'WASD' to move");
        // interaction
        tutorials.Add("Interaction", "Use 'E' to interact with glowing objects");
        // opening/closing phone
        tutorials.Add("Open/Close Phone", "Use 'TAB' to open or close your phone");
    }

    // Update is called once per frame
   // void Update()
    //{
        /*switch (currTutor)
        {
            case 0: // movement tutorial
                if (!iEnumActive && Keyboard.current[Key.W].isPressed)
                {
                    StartCoroutine(TutorialDuration());
                }
                break;

            case 1: // interaction tutorial
                if (!iEnumActive && Keyboard.current[Key.E].isPressed)
                {
                    StartCoroutine(TutorialDuration());
                }
                break;

            case 2:
            case 3: // phone navigation and opening tutorial
                if (!iEnumActive && Keyboard.current[Key.Tab].isPressed)
                {
                    StartCoroutine(TutorialDuration());
                }
                break;
        }*/
    //}

    // used find the tutorial in the the dictionary
    public void StartTutorial(string name)
    {
        // if the dictionary contains the key, show the tutorial
        if (tutorials.ContainsKey(name))
        {
            FindObjectOfType<NotificationManager>().ShowTutorial(name, tutorials[name]);
        }
    }

    // used to set the gameobject inactive after its condition's been met and activates the next tutorial text
    public IEnumerator TutorialDuration()
    {
        float duration = 2f;

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
