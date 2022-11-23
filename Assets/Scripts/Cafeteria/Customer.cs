using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public bool isSatisfied;
    public string order; // is set when the npc is spawned

    // Start is called before the first frame update
    void Start()
    {
        isSatisfied = false;
    }
}
