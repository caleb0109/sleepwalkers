using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach script to the object housing all the node positions 
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
    }

    // move the item location to that specific node based on closest node to players current position
    public void MoveItemToNode(GameObject item)
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
        if(CheckForOtherLocs(itemLoc))
        {
            item.transform.position = FindClosestLocation(item.transform, pIndex).position;
        }
        else // otherwise, set it to the current itemLoc position
        {
            item.transform.position = itemLoc.position;
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

    private Transform FindClosestLocation(Transform item, int placementIndex)
    {
        Transform multiLoc= placements[placementIndex];
        Transform playerLoc = GameObject.Find("Yuichi").transform;
        Transform closestLoc = null;

        // compare the x and y positions with the player location
        /*for (int i = 0; i < multiLoc.childCount; i++)
        {
            if (multiLoc[i].position.x + 5 == )
            {

            }

            if (multiLoc[i].position.y + 5 == )
            {

            }
        }*/

        return closestLoc;
    }
}
