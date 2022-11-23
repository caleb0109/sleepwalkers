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
    private Dictionary<string, Sprite> foodOptions; // used to determine which sprite to put down
    private List<GameObject> customers;

    // Start is called before the first frame update
    void Start()
    {
        ordersCompleted = 0;
        numNpcs = 3;
        nSystem = FindObjectOfType<Nodes>();

        foodOptions = new Dictionary<string, Sprite>();
        customers = new List<GameObject>();

        SpawnNpcs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnNpcs()
    {
        if (customers.Count < numNpcs)
        {
            for (int i = 0; i < numNpcs; i++)
            {
                if (customers.Count - 1 < i)
                {
                    customers.Add(Instantiate(npcPrefab, nSystem.ReturnRandomNodePos("students"), Quaternion.identity));
                    customers[i].AddComponent<Customer>();
                }
            }
        }
    }

    // leaves the food for the player to pick up
    private void SetOrderDown()
    {
        
    }

    // destroys the Npcs after they eat
    private void LeaveCafeteria()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            if (customers[i].GetComponent<Customer>().isSatisfied)
            {
                Destroy(customers[i]);
            }
        }
    }

    IEnumerator FoodGen(string action)
    {
        float duration = 3.0f;

        while (duration > 0.0f)
        {
            duration -= Time.deltaTime;
            if (duration < 0.1f)
            {
                if (action == "cook")
                {
                    SetOrderDown();
                }
                else if (action == "eat")
                {
                    LeaveCafeteria();
                }
            }
        }

        yield return null;
    }
}
