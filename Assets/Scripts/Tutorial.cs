using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<GameObject> tutorials;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // used to set the gameobject inactive after its condition's been met and activates the next tutorial text
    IEnumerator TutorialDuration()
    {
        yield return null;
    }
}
