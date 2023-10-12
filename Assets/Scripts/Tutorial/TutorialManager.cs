using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region Variables
    private List<Tutorial> tutorials; // used to get the children tutorials that'll be played in the specific scene
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // initialize lists
        tutorials = new List<Tutorial>();

        // gets the number of child tutorials and add them to the list
        for (int i = 0; i < this.transform.childCount; i++)
        {
            tutorials.Add(this.transform.GetChild(i).gameObject.GetComponent<Tutorial>());
        }
    }

    void FixedUpdate()
    {
        foreach (Tutorial t in tutorials)
        {
            if (!t.Completed)
            {
                t.StartTutorial();
            }
        }
    }

    // Only used if tutorial needs to stay on the screen for a bit longer after it's been completed
    IEnumerator MoveToNextTutorial()
    {
        yield return null;
    }
}
