using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject obj;
    private GameObject player;

    // used to store data from player in past scene
    private GameObject playerData;
    private GameObject gManager;

    public void Awake()
    {
            obj = this.gameObject;
            StorePlayerData();

            SetPlayerData();

            Destroy(this.gameObject);
            return;

        DontDestroyOnLoad(obj);

        player = GameObject.Find("Yuichi");
    }

    public void StorePlayerData()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            if (child.name == "Yuichi")
            {
                // store the data of the player
                playerData = child.gameObject;
            }

            if (this.transform.GetChild(i).name == "GameManager")
            {
                // store the data of the GameManager
                gManager = child.gameObject;
            }
        }


    }

    public void SetPlayerData()
    {
        player = GameObject.Find("Yuichi");

        Inventory prevInvent = playerData.GetComponent<Inventory>();

        Inventory currInventory = player.GetComponent<Inventory>();
        currInventory.items = prevInvent.items;
        currInventory.itemSprites = prevInvent.itemSprites;

        currInventory.Update_UI();

       
    }
}
