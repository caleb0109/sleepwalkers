using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    private int slotNum;
    private Button[] slots;

    public GameObject overwritePrompt;

    // Start is called before the first frame update
    void Start()
    {
        slots = new Button[3];

    }

    // checks if the slot selected is empty
    public void CheckEmptySlot()
    {

    }

    // create a new save file with the selected slot
    public void StartNewSave()
    {

    }

    // overwrite save file with the selected slot
    public void OverwriteSave()
    {

    }

    // load game from the selected slot's save file
    public void LoadGame()
    {

    }

    // prompts the user if they want to change
    public void TogglePrompt()
    {
        overwritePrompt.SetActive(!overwritePrompt.activeInHierarchy);
    }
}
