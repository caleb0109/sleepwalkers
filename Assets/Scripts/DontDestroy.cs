using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject obj;
    private GameObject player;
    private Inventory prevInvent;

    public void Awake()
    {
        if (obj == null)
        {
            obj = this.gameObject;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(obj);

        player = GameObject.Find("Yuichi");
    }

    public void StorePlayerData()
    {
        prevInvent = player.GetComponent<Inventory>();
    }

    public void SetPlayerData()
    {
        player = GameObject.Find("Yuichi");

        /*Inventory currInventory = player.GetComponent<Inventory>();
        currInventory.items = prevInvent.items;
        currInventory.itemSprites = prevInvent.itemSprites;

        currInventory.Update_UI();*/
    }
}
