using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Programmer: Jessica Niem
/// Date: 
/// Description: Attach script to the object housing all the node positions
/// </summary>
public class Nodes : MonoBehaviour
{
    private List<Transform> placements; // holds all the locs the items can be placed

    // Start is called before the first frame update
    void Start()
    {
        placements = new List<Transform>();

        // get all the children of the placements object
        for (int i = 0; i < this.transform.childCount; i++)
        {
            placements.Add(this.transform.GetChild(i));
        }

        CinemachineConfiner camConfiner = GameObject.Find("main").GetComponent<CinemachineConfiner>();

        //camConfiner.bounding2dShape = GameObject.Find("camConfine").GetComponent<PolygonCollider2d>();

        GameObject player = GameObject.Find("Yuichi");

        if (!player.GetComponent<SpriteRenderer>().enabled)
        {
            player.GetComponent<SpriteRenderer>().enabled = true;
        }

       MoveItemToNode(player);
    }

    // get the number of possible placements in the map
    public int GetPlacementCount(string listName)
    {

        for (int i = 0; i < placements.Count; i++)
        {
            if (placements[i].name == listName)
            {
                if (CheckForOtherLocs(placements[i]))
                {
                    return placements[i].childCount;
                }
                else
                {
                    return 1;
                }
            }
        }


        return 0; // if it couldn't find any, return 0
    }

    // find if there's placement nodes
    private Transform FindPlacement(string name)
    {
        foreach (Transform t in placements)
        {
            if (t.name == name)
            {
                return t;
            }
        }

        // return nothing if it doesn't exsist
        return null;
    }

    // returns a random position for the items
    public Vector3 ReturnRandomNodePos(string objType)
    {
        List<Transform> locList = new List<Transform>();

        Transform tObj = FindPlacement(objType);
        if (tObj != null) {
            for (int i = 0; i < tObj.childCount; i++)
            {
                locList.Add(tObj.GetChild(i));
            }
        }

        return locList[Random.Range(0, locList.Count)].position;
    }

    public void PlaceRandomIn(string confiner, GameObject itemToPlace)
    {
        Transform place = FindPlacement(confiner);

        // TEMP: need to get it to calculate based on the width and height
        float xMinMax = place.position.x / 2;
        float yMinMax = place.position.y / 2;

        itemToPlace.transform.position = new Vector3(Random.Range(-xMinMax, xMinMax), Random.Range(-yMinMax, yMinMax), 0);
    }

    // move the item location to that specific node based on closest node to players current position
    public void MoveItemToNode(GameObject item)
    {
        if (item != null)
        {

            Transform itemLoc = null; // TODO: rename variable
            int pIndex = 0; // used in FindClosestLocation(...)

            // go through each transform in the placements list
            foreach (Transform t in placements)
            {
                // if the item name contains the location object name, assign the itemLoc to t and break
                if (item.name.Contains(t.gameObject.name))
                {
                    itemLoc = t;
                    break;
                }

                pIndex++;
            }

            // if there's other locations, find the closest location and set the position to it
            if (CheckForOtherLocs(itemLoc))
            {
                item.transform.position = FindClosestLocation(pIndex, GameObject.Find("Yuichi"));
            }
            else // otherwise, set it to the current itemLoc position
            {
                item.transform.position = itemLoc.position;
            }
        }
    }

    // second overload for the above method for the cafeteria minigmae
    public void MoveItemToNode(GameObject itemToMove, string objType, GameObject interactedItem)
    {
        Transform itemLoc = null; // TODO: rename variable
        int pIndex = 0; // used in FindClosestLocation(...)

        // go through each transform in the placements list
        foreach (Transform t in placements)
        {
            // if the item name contains the location object name, assign the itemLoc to t and break
            if (objType.Contains(t.gameObject.name))
            {
                itemLoc = t;
                break;
            }

            pIndex++;
        }

        // if there's other locations, find the closest location and set the position to it
        if (CheckForOtherLocs(itemLoc))
        {
            itemToMove.transform.position = FindClosestLocation(pIndex, interactedItem);
        }
        else // otherwise, set it to the current itemLoc position
        {
            itemToMove.transform.position = itemLoc.position;
        }
    }

    // checks if the item has other locations to can be placed at then make it be able to be picked up
    private bool CheckForOtherLocs(Transform itemLoc)
    {
        // if the childCount of the transform is more than 0, then return true otherwise return false
        if (itemLoc.childCount > 0)
        {
            return true;
        }

        return false;
    }

    // finds the closest placement location to the itemToCompare's position
    private Vector3 FindClosestLocation(int placementIndex, GameObject itemToCompare)
    {
        Transform multiLoc= placements[placementIndex];
        Vector3 itemLoc = itemToCompare.transform.position;
        Vector3 closestLoc = new Vector3(0, 0, 0);

        Vector3 smallestDist = new Vector3(10, 10, 0); // used to see which location is closest

        // compare the x and y positions with the player location
        for (int i = 0; i < multiLoc.childCount; i++)
        {
            Vector3 child = multiLoc.GetChild(i).transform.position;
            //Debug.Log(multiLoc.GetChild(i).gameObject.name + " " + multiLoc.GetChild(i).position);            

            // stores the distance
            Vector3 distance = new Vector3(0,0,0);

            distance.x = CalculateDistance(child.x, itemLoc.x);
            distance.y = CalculateDistance(child.y, itemLoc.y);

            // checks if the difference is within a certain radius
            if ( distance.x > -5 && distance.y > -5 
                && distance.x < 5 && distance.y < 5)
            {
                if (smallestDist.x > distance.x &&  smallestDist.y > distance.y)
                {
                    smallestDist = distance;
                    closestLoc = child;
                }
            }

        }

        return closestLoc;
    }

    // calculate the distance for the x and y pos for the itemPos and playerPos
    private float CalculateDistance(float itemPos, float playerPos)
    {
        float dist = 0.0f;

        // if itemPos is negative and playerPos is positive
        if (itemPos < 0 && playerPos > 0) 
        {
            dist = -itemPos + playerPos; // (-i + p) change itemPos to positive and add to playerPos
        }
        // if playerPos is negative and itemPos is positive
        else if (playerPos < 0 && itemPos > 0) 
        {
            dist = -playerPos + itemPos; // (-p + i) change playerPos to positive and add to itemPos
        }
        // if both positions are negative
        else if (itemPos < 0 && playerPos < 0)
        {
            if (itemPos < playerPos) // if itemPos is the lower number
            {
                dist = -itemPos - (-playerPos); // change both to positive and subtract playerPos from itemPos
            }
            else // if playerPos is the lower number
            {
                dist = -playerPos - (-itemPos); // change both to positive and subtract itemPos from playerPos
            }
        }
        // if the itemPosition is greater than the player's
        else if (itemPos > playerPos)
        {
            dist = itemPos - playerPos; // subtract the bigger num to get distance
        }
        // vice versa
        else
        {
            dist = playerPos - itemPos; // ^ same as above
        }

        return dist;
    } 
}
