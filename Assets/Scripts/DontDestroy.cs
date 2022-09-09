using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private static GameObject obj;
    private GameObject startingPos;
    private GameObject player;
    private Scene scene;

    // used to store data from player in past scene
    private GameObject playerData;
    private GameObject gManager;

    public void Awake()
    {
        obj = this.gameObject;
        StorePlayerData();

        SetPlayerData();

        DontDestroyOnLoad(obj);

        player = GameObject.Find("Yuichi");
        startingPos = GameObject.Find("startingPoint");
        if(scene.isLoaded)
        {
            player.transform.position = startingPos.transform.position;
        }
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
    public void UpdatePos()
    {
        player.transform.position = startingPos.transform.position;
    }
}
