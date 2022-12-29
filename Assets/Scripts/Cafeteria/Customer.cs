using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public bool isSatisfied;
    public string order; // is set when the npc is spawned
    public GameObject dishEaten; // holds the object the npc is eating

    public float patience;

    // Start is called before the first frame update
    void Start()
    {
        isSatisfied = false;
        dishEaten = null;
        patience = 10.0f;

        StartCoroutine(PatienceMeter());
    }

    private void ThrowTrash()
    {
        Debug.Log("I didn't get my food >:(");
    }


    IEnumerator PatienceMeter()
    {
        while (patience > 0.0f)
        {
            patience -= Time.deltaTime;

            if (patience < 0.01f)
            {
                ThrowTrash();
            }

            yield return null;
        }
    }
}
