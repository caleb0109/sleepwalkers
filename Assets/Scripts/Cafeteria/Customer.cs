using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer: Jessica Niem
/// Date:
/// Description: Holds the data of individual "customers" and keeps track of patience and what the npc wants to "eat"
/// </summary>

public class Customer : MonoBehaviour
{
    public string order; // is set when the npc is spawned
    public GameObject dishEaten; // holds the object the npc is eating

    public float patience;
    public Bars patienceBar;

    // Start is called before the first frame update
    void Start()
    {
        dishEaten = null;
        patience = 10.0f;

        patienceBar = this.GetComponent<Bars>();

        StartCoroutine(PatienceMeter());
    }

    public IEnumerator PatienceMeter()
    {
        int prevPatience = (int)patience;
        while (patience > 0.0f)
        {
            patience -= Time.deltaTime;

            // prevents the meter from updating every frame and only updates when a second passes
            if (prevPatience > (int)patience)
            {
                patienceBar.ShowMeter();
                prevPatience = (int)patience;
            }

            if (patience < 0.01f)
            {
                Debug.Log(this.gameObject.name + ": I didn't get my food >:(");
                FindObjectOfType<CafeteriaMinigame>().LeaveCafeteria();
            }

            yield return null;
        }
    }
}
