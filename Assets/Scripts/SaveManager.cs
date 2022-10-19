using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private int slotNum;
    private Button[] slots;

    private string path;

    public GameObject overwritePrompt;

    // Start is called before the first frame update
    void Start()
    {
        slots = new Button[3];
        path = "Files/Save";
        Debug.Log(Resources.LoadAsync<TextAsset>(path));
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
