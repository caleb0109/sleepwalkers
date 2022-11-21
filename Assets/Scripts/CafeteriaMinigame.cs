using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeteriaMinigame : MonoBehaviour
{
    public GameObject npcPrefab;
    public GameObject orderPrefab;

    const int totalOrders = 24;
    const int orderFrequency = 6; // used to increment frequency 

    private int ordersCompleted;
    private int numNpcs;
    private Nodes nSystem;
    private Dictionary<string, Sprite> foodOptions; // used to 

    // Start is called before the first frame update
    void Start()
    {
        ordersCompleted = 0;
        numNpcs = 3;
        nSystem = FindObjectOfType<Nodes>();

        foodOptions = new Dictionary<string, Sprite>(); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetOrderDown()
    {
        
    }

    IEnumerator CookOrders()
    {
        float duration = 3.0f;

        while (duration > 0.0f)
        {
            duration -= Time.deltaTime;
            if (duration < 0.1f)
            {
                SetOrderDown();
            }
        }

        yield return null;
    }
}
