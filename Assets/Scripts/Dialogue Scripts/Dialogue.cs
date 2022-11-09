using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Dialogue
{
    private string name; // set the default to Yuichi, since we only use this for him
    private Sprite defaultSprite;

    [TextArea(3, 10)]
    public List<string> sentences; // used for interactions with items or one off lines
    public TextAsset diaFile;

    private List<Sprite> charaSprites;

    // used to store the different facial expressions
    private Dictionary<string, Dictionary<string, Sprite>> expressions; 

    private List<string> charaNames;
    private List<string> charaLines;

    // used to store sentences tied to conditionals
    private List<List<string>> conditionalSentences;

    #region Properties
    public List<string> CharaNames { get { return charaNames; } }
    public List<string> CharaLines { get { return charaLines; } }
    public List<Sprite> CharaSprites { get { return charaSprites; } }
    public string Name { get { return name; } }
    public Sprite DefaultSprite { get { return defaultSprite; } }

    public Dictionary<string, Dictionary<string, Sprite>> Expressions { get { return expressions; } }
    #endregion

    public void Start()
    {
        name = "Yuichi";
        defaultSprite = Resources.Load<Sprite>("Sprites/pfps/Yuichi/Neutral");

        expressions = new Dictionary<string, Dictionary<string, Sprite>>();

        conditionalSentences = new List<List<string>>();

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

        foreach (string n in charaNames)
        {
            LoadSprites(string.Format("Sprites/pfps/", n), n);
        }

        int section = -1;
        int prevSection;
        for (int i = 0; i < lines.Count; i++)
        {
            prevSection = section;

            if (lines[i].Contains("<<"))
            {
                section++;
            }
            else if (section == -1)
            {
                charaLines.Add(lines[i]);
            }
            else
            {
                if (prevSection != section)
                {
                    conditionalSentences.Add(new List<string>());
                }

                conditionalSentences[section].Add(lines[i]);
            }
        }
    }

    private void LoadSprites(string path, string name)
    {
        object[] temp = Resources.LoadAll(path, typeof(Sprite));
        expressions.Add(name, new Dictionary<string, Sprite>());

        for (int i = 0; i < temp.Length; i++)
        {
            Sprite emotion = (Sprite)temp[i];
            expressions[name].Add(emotion.name, emotion);
        }
    }

    public Sprite FindExpression(string name, string expression)
    {
        return expressions[name][expression];
    }
}
