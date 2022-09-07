using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    private List<GameObject> tutorials;
    private int currTutor;
    private bool iEnumActive;

    // Start is called before the first frame update
    void Start()
    {
        tutorials = new List<GameObject>();
        currTutor = 0;
        iEnumActive = false;

        // get the child of this gameObject and add them to the list of tutorials
        for (int i = 0; i < this.transform.childCount; i++)
        {
            tutorials.Add(this.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currTutor)
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
        }
    }

    // used to set the gameobject inactive after its condition's been met and activates the next tutorial text
    IEnumerator TutorialDuration()
    {
        float duration = 2f;
        iEnumActive = true;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            
            if (duration < 0f)
            {
                // make tutorial at curr index disappear and make the next one appear
                tutorials[currTutor].SetActive(false);
                currTutor++;

                // if there's anymore tutorials, set active
                if (currTutor < tutorials.Count)
                {
                    tutorials[currTutor].SetActive(true);
                    iEnumActive = false;
                }
            }

            yield return null;
        }

    }
}
