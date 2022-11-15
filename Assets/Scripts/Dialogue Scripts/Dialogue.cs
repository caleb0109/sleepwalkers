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

        expressions.Add(name, new Dictionary<string, Sprite>());
        expressions[name].Add("Neutral", defaultSprite);

        conditionalSentences = new List<List<string>>();

        if (diaFile) // if there's a file attached, load it
        {
            charaNames = new List<string>();
            charaLines = new List<string>();
            LoadDialogueFile();
        }
    }

    public void ResetAndLoadNewDialogue()
    {
        charaNames.Clear();
        charaLines.Clear();
        LoadDialogueFile();
    }

    private void LoadDialogueFile()
    {
        List<string> lines = new List<string>(diaFile.text.Split('\n')); // get all the lines in the file

        if (lines[0].Contains(",") || !lines[0].Contains("|"))
        {
            charaNames.AddRange(lines[0].Split(',')); // get all the names from the file

            lines.RemoveAt(0); // removes the list of names
        }
        
        // goes through each name and finds the folder based on the character's name
        foreach (string n in charaNames)
        {
            if (n == "???")
            {
                LoadSprites("Sprites/pfps/Enemy", n); //TODO: fix this for other characters with '???' names
            }
            else if (n.Contains("Student"))
            {
                LoadSprites("Sprites/pfps/Student", n); // TODO: unhardcode the loading
            }
            else
            {
                LoadSprites(new string("Sprites/pfps/" + n), n);
            }
        }

        charaLines.AddRange(lines);
    }


    // loads all the emotion sprites for each character
    private void LoadSprites(string path, string name)
    {
        // searches the resources folder for the sprites and adds it to the array
        object[] temp = Resources.LoadAll(path, typeof(Sprite));

        if (name != "Yuichi" && !expressions.ContainsKey(name))
        {
            expressions.Add(name, new Dictionary<string, Sprite>()); // initializes the dictionary inside the dictionary
        }

        // add each sprite to the character's inner dictionary
        for (int i = 0; i < temp.Length; i++)
        {
            Sprite emotion = (Sprite)temp[i];
            if (!expressions[name].ContainsKey(emotion.name))
            {
                expressions[name].Add(emotion.name, emotion);
            }
        }
    }

    // returns the sprite of the expression
    public Sprite FindExpression(string name, string expression)
    {
        Sprite returned = defaultSprite; // returns the default if it can't find the expression

        if (expressions[name].ContainsKey(expression))
        {
            returned = expressions[name][expression];
        }

        return returned;
    }
}
