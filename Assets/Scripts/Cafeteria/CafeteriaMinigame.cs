using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeteriaMinigame : MonoBehaviour
{
    public GameObject npcPrefab;
    public List<GameObject> orderPrefabs; // a list of all the orders
    public List<GameObject> orderBubbles; // a list of all the bubbles that accompany the order
    public GameObject itemInHand = null; // the current dish the player is holding (set in the Interactable script)
    public bool miniGameStarted;

    const int totalOrders = 20;
    const int orderFrequency = 6; // used to increment frequency 

    private int ordersCompleted;
    private int numNpcs;
    private Nodes nSystem;
    private List<GameObject> customers;
    private List<GameObject> dishes; // keeps track of the dishes that are placed on the map
    private List<GameObject> ordersReady; // keeps track of the number of orders placed down ready for pick up
    private Queue<GameObject> orderBacklog;

    // Start is called before the first frame update
    void Start()
    {
        ordersCompleted = 0;
        numNpcs = 3;
        nSystem = FindObjectOfType<Nodes>();

        customers = new List<GameObject>();
        dishes = new List<GameObject>();
        ordersReady = new List<GameObject>();
        orderBacklog = new Queue<GameObject>();

        miniGameStarted = false;
    }

    void Update()
    {
        if (miniGameStarted)
        {
            // once all the orders are complete, destroy itself
            if (ordersCompleted == totalOrders)
            {
                Debug.Log("Minigame Completed");
                for (int i = 0; i < customers.Count; i++)
                {
                    Destroy(customers[i]);
                    customers.RemoveAt(i);

                    /*Destroy(dishes[i]);
                    dishes.RemoveAt(i);

                    if (i < 5)
                    {
                        Destroy(ordersReady[i]);
                    }*/
                }

                Destroy(this.gameObject);
            }
            else
            {
                SpawnNpcs();
                LeaveCafeteria(); // placed here for testing
            }
        }
    }

    // creates the npcs using the prefab and add the Customer script to it while chosing a random
    // order to put for the npc to want
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
                    Customer c = customers[i].GetComponent<Customer>();
                    c.order = orderPrefabs[Random.Range(0,1)].name;
                    
                    for (int j = 0; j < orderBubbles.Count; j++)
                    {
                        if (orderBubbles[j].name.Contains(c.order))
                        {
                          Instantiate(orderBubbles[j], customers[i].transform);
                        }
                    }

                }
            }

            CheckIfItemsAreOverlapping(customers, "students");
        }
    }

    public void PlaceOrder(GameObject customer)
    {
        foreach (GameObject g in customers)
        {
            if (g.transform.position == customer.transform.position)
            {
                if (itemInHand.name == customer.GetComponent<Customer>().order)
                {
                    Destroy(customer.transform.GetChild(1)); // destroys the bubble

                    g.GetComponent<Customer>().isSatisfied = true;

                    dishes.Add(itemInHand);
                    nSystem.MoveItemToNode(itemInHand);
                    itemInHand = null; // after the item has been placed, there is no item being held

                    StopAllCoroutines();
                    StartCoroutine(FoodGen("eat"));
                    break;
                }
            }
        }
    }

    // leaves the food for the player to pick up
    private void SetOrderDown()
    {
        // don't go over 5 at a time
        if (ordersReady.Count < 5)
        {
            for (int i = 0; i < ordersReady.Count; i++)
            {
                if (ordersReady[i] == null)
                {
                    ordersReady[i] = Instantiate(orderPrefabs[Random.Range(0,1)], nSystem.ReturnRandomNodePos("orders"), Quaternion.identity);
                    break;
                }
            }

            CheckIfItemsAreOverlapping(ordersReady, "orders");
        }
    }

    // checks if any items are spawned on the same node and move them
    private void CheckIfItemsAreOverlapping(List<GameObject> objs, string placementName)
    {
        for (int i = 0; i < objs.Count; i++)
        {
            for (int j = 0; j < objs.Count; j++)
            {
                // only check if i is different from j
                if (i != j)
                {
                    if (objs[i].transform.position == objs[j].transform.position)
                    {
                        objs[j].transform.position = nSystem.ReturnRandomNodePos(placementName);
                    }
                }
            }
        }
    }

    // destroys the Npcs after they eat and removes them from the list
    private void LeaveCafeteria()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            if (customers[i].GetComponent<Customer>().isSatisfied)
            {
                Destroy(customers[i]);
                customers.RemoveAt(i);
                ordersCompleted++;

                Destroy(dishes[i]);
                dishes.RemoveAt(i);
            }
        }
    }

    // timer for how long it takes to cook or eat each food
    public IEnumerator FoodGen(string action)
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
