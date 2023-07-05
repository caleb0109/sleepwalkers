using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer: Jessica Niem
/// Date: 3/18/23
/// Decription: Takes care of spawning items and attaching the information to the object
/// </summary>
public class ItemManager : MonoBehaviour
{
    public enum Item
    {
        None,
        Placeable,
        Useable,
        Weapon,
        Food
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateItem()
    {

    }

    public void LoadInventory()
    {
        // check inventory files
    }
}
