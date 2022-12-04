using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeteriaMinigame : MonoBehaviour
{
    public GameObject npcPrefab;
    public List<GameObject> orderPrefabs; // a list of all the orders
    public List<GameObject> orderBubbles; // a list of all the bubbles that accompany the order
    public GameObject itemInHand = null; // the current dish the player is holding (set in the Interactable script)

    const int totalOrders = 10;
    const int orderFrequency = 6; // used to increment frequency 

    private bool miniGameStarted;
    private int ordersCompleted;
    private int numNpcs;
    private Nodes nSystem;
    private GameObject chef;
    private List<GameObject> customers;
    private List<GameObject> ordersReady; // keeps track of the number of orders placed down ready for pick up
    private Queue<GameObject> orderBacklog;

    // Start is called before the first frame update
    void Start()
    {
        ordersCompleted = 0;
        numNpcs = 3;
        nSystem = FindObjectOfType<Nodes>();

        chef = GameObject.Find("chef");

        customers = new List<GameObject>();
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
                    chef.SetActive(true);
                    Customer cData = customers[i].GetComponent<Customer>();
                    if (cData.dishEaten != null)
                    {
                        Destroy(cData.dishEaten);
                    }

                    Destroy(customers[i]);
                    customers.RemoveAt(i);

                    
                    if (i < 5)
                    {
                        Destroy(ordersReady[i]);
                        ordersReady.RemoveAt(i);
                    }
                }

                Destroy(this.gameObject);
            }
            else
            {
                SpawnNpcs();
            }
        }
    }

    public void StartMiniGame()
    {
        miniGameStarted = true;
        chef.SetActive(false);
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
                    GameObject cOrder = orderPrefabs[Random.Range(0, orderPrefabs.Count)];
                    c.order = cOrder.name;
                    
                    for (int j = 0; j < orderBubbles.Count; j++)
                    {
                        if (orderBubbles[j].name.Contains(c.order))
                        {
                          Instantiate(orderBubbles[j], customers[i].transform);
                        }
                    }

                    orderBacklog.Enqueue(cOrder);
                }
            }

            CheckIfItemsAreOverlapping(customers, "students");
            StartCoroutine(FoodGen("cook"));
        }
    }

    // used to put the order down in front of the customer
    public void PlaceOrder(GameObject customer)
    {
        foreach (GameObject g in customers)
        {
            if (g.transform.position == customer.transform.position)
            {
                if (itemInHand.name.Contains(customer.GetComponent<Customer>().order))
                {
                    Destroy(customer.transform.GetChild(1).gameObject); // destroys the bubble

                    // gets the customer script and sets the data
                    Customer customerData = g.GetComponent<Customer>();
                    customerData.isSatisfied = true;
                    customerData.dishEaten = itemInHand;
                    
                    nSystem.MoveItemToNode(itemInHand, "dishes", g);
                    itemInHand.SetActive(true);
                    itemInHand = null; // after the item has been placed, there is no item being held

                    StartCoroutine(FoodGen("eat")); // starts the timer
                    break;
                }
            }
        }
    }

    // used to pick up the order from the order station
    public void PickUpOrder(GameObject order)
    {
        for (int i = 0; i < ordersReady.Count; i++)
        {
            if (ordersReady[i] == order)
            {
                ordersReady.RemoveAt(i);
                break;
            }
        }

        itemInHand = order;
        order.SetActive(false);
    }

    // leaves the food for the player to pick up
    private void SetOrderDown(GameObject order)
    {
        ordersReady.Add(Instantiate(order, nSystem.ReturnRandomNodePos("orders"), Quaternion.identity));

        CheckIfItemsAreOverlapping(ordersReady, "orders");
        StopAllCoroutines();
        StartCoroutine(FoodGen("cook"));
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
            Customer cData = customers[i].GetComponent<Customer>();

            if (cData.isSatisfied)
            {
                Destroy(cData.dishEaten);
                Destroy(customers[i]);
                customers.RemoveAt(i);
                ordersCompleted++;
            }
        }
    }

    // timer for how long it takes to cook or eat each food
  IEnumerator FoodGen(string action)
    {
        Debug.Log(action);
        float duration;

        if (action == "cook")
        {
            duration = 3.0f;
        }
        else
        {
            duration = 5.0f;
        }

        while (duration > 0.0f)
        {
            duration -= Time.deltaTime;
            if (duration < 0.01f)
            {
                if (action == "cook")
                {
                    // don't go over 5 at a time
                    if (orderBacklog.Count > 0 && ordersReady.Count < 5)
                    {
                        SetOrderDown(orderBacklog.Dequeue());
                    }
                }
                else
                {
                    LeaveCafeteria();
                }
            }

            yield return null;
        }
    }
}
