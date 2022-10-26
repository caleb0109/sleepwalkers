using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    private string name; // set the default to Yuichi, since we only use this for him
    private Sprite sprite;

    [TextArea(3, 10)]
    public List<string> sentences; // used for interactions with items or one off lines
    public TextAsset diaFile;

    public string currAct = "Intro";

    // find efficient way later
    private List<Sprite> charaSprites;

    // used to store the different facial expressions
    /* Order Stored:
     * Neutral
     * Happy
     * Angry
     * Sad
     * Shocked
     * Fear
     */
    // each npc holds their own sprites stored like above
    // then add to dictionary in start
    // if facial expression doesn't exsist, place a placeholder in that loc
    private Dictionary<string, List<Sprite>> expressions; 

    private List<string> charaNames;
    private List<string> charaLines;

    #region Properties
    public List<string> CharaNames { get { return charaNames; } }
    public List<string> CharaLines { get { return charaLines; } }
    public List<Sprite> CharaSprites { get { return charaSprites; } }
    public string Name { get { return name; } }
    public Sprite Sprite {
        get { return sprite; } 
        set { sprite = value; }
    }
    #endregion

    public void Start()
    {
        name = "Yuichi";
        sprite = Resources.Load<Sprite>("Sprites/pfps/Yuichi"); // loads Yuichi's

        string path = "Sprites/pfps/" + currAct;

        // loads all the sprites from the resources folder
        object[] temp = Resources.LoadAll(path, typeof(Sprite));

        charaSprites = new List<Sprite>();


        charaSprites.Add(sprite);

        // convert all objects to Sprites
         for (int i = 0; i < temp.Length; i++)
        {
            charaSprites.Add((Sprite)temp[i]);
        }

        if (diaFile) // if there's a file attached, load it
        {
            charaNames = new List<string>();
            charaLines = new List<string>();
            LoadDialogueFile();
        }
    }

    private void LoadDialogueFile()
    {
        List<string> lines = new List<string>(diaFile.text.Split('\n')); // get all the lines in the file

        if (lines[0].Contains(",") || !lines[0].Contains("|"))
        {
            charaNames.AddRange(lines[0].Split(',')); // get all the names from the file

            lines.RemoveAt(0); // removes the list of names
        }

        charaLines.AddRange(lines);
    }

}
