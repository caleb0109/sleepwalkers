using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    const int autoSaveCap = 15; // cap for how many autosaves there can be

    private Inventory pInvent;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        SaveGame("autoSave");
    }

    // used for checkpoints in story, location, etc.
    // takes a string that helps with determining how to save the data
    public void SaveGame(string saveType)
    {
        Debug.Log("I'm saving player inventory and the player itself");
        // if it's an auto save
        if (saveType == "autoSave")
        {
            player = GameObject.Find("Yuichi");
            pInvent = player.GetComponent<Inventory>();
        }
        else // if it's manual saving
        {

        }
    }

    // manual loading and when player dies, get the last known checkpoint
    public void LoadGame()
    {
        Debug.Log("I'm loading the data into the new player");
        GameObject newPobj = GameObject.Find("Yuichi");
        newPobj.transform.position = player.transform.position;
        Inventory newPinvent = newPobj.GetComponent<Inventory>();
        newPinvent = pInvent;

        //newPinvent.Update_UI();
    }
}
