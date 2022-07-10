using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    private string name; // set the default to Yuichi, since we only use this for him
    public Sprite sprite;

    [TextArea(3, 10)]
    public string[] sentences; // used for interactions with items or one off lines
    public TextAsset diaFile;

    // find efficient way later
    public List<Sprite> charaSprites;

    private List<string> charaNames;
    private List<string> charaLines;

    #region Properties
    public List<string> CharaNames { get { return charaNames; } }
    public List<string> CharaLines { get { return charaLines; } }
    public string Name { get { return name; } }
    #endregion

    public void Start()
    {
        name = "Yuichi";

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

        if (lines[0].Contains(","))
        {
            charaNames.AddRange(lines[0].Split(',')); // get all the names from the file

            lines.RemoveAt(0); // removes the list of names
        }

        charaLines.AddRange(lines);
    }

}
