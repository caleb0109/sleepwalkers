// Base code from https://www.youtube.com/watch?v=_nRzoTzeyxU 

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour
{
    #region Variables
    // variables other scripts will use to change specific data to display
    public Text nameText;
    public Text dialogueText;
    public Image imgSprite;
    public Animator animator;
    public GameObject diaPrompt;
    [HideInInspector]public bool isSpeaking;
    [HideInInspector]public bool isTyping;
    [HideInInspector]public bool autoDia;

    // variables used to display the sentences
    private Queue<string> sentences;
    private Dialogue dialogueHolder;
    private string currSentence;

    // battle variables
    private bool startBattle;
    private GameObject gObj; // passes game object data to battle scripts

    private float typingSpeed; // used to determine spacing between each letter typed

    // variables for playing sfx
    public List<AudioClip> charaSpeech;
    private Dictionary<string, List<AudioClip>> charasInDia;
    private AudioSource audSrc;
    #endregion

    void Start()
    {
        charasInDia = new Dictionary<string, List<AudioClip>>();
        audSrc = this.gameObject.GetComponent<AudioSource>();
        sentences = new Queue<string>();
        isSpeaking = false;
        isTyping = false;
        startBattle = false;
        typingSpeed = 2f; // default typing speed
    }


    // used for normal dialogue between characters or with Yuichi
    public void StartDialogue(Dialogue dia, bool battle, GameObject obj)
    {
        dialogueHolder = dia;
        startBattle = battle;
        gObj = obj;

        StartDialogueAnimation(false);

        GetDialogue();

        DisplayNextSentence(); // starts the first sentence
    }

    // iterates throught the dialogue for the battle sequence
    public void StartBattleDialogue(Dialogue dia)
    {
        dialogueHolder = dia;
        autoDia = false;

        // clears the queue of sentences if there are any
        if (sentences != null)
        {
            sentences.Clear();
        }

        // puts each sentence into the queue
        foreach (string sent in dia.sentences)
        {
            sentences.Enqueue(sent);
        }

        currSentence = sentences.Dequeue();

        if (currSentence.Contains("|"))
        {
            string[] split = currSentence.Split('|');
            currSentence = split[1];
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence());
    }

    // AutoPlays Dialogue
    public void StartAutoDialogue(Dialogue dia)
    {
        dialogueHolder = dia;

        StartDialogueAnimation(true);
        diaPrompt.SetActive(false);

        GetDialogue();

        DisplayNextSentence(); // displays the first sentence of the auto dialogue
    }

    // displays the sentence
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // used to display the sentence and type out the letters
        currSentence = sentences.Dequeue();
        
        if (currSentence.Contains("|"))
        {
            if (!nameText.gameObject.activeInHierarchy)
            {
                nameText.gameObject.SetActive(true);
                imgSprite.gameObject.SetActive(true);
            }
            string[] split = currSentence.Split('|');
            currSentence = split[1];

            string[] currEmotion = split[0].Split('-'); // written to file like = "CharaInitial-Emotion"

            // look for the name and set the correct sprite and name for the line
            for (int i = 0; i < dialogueHolder.CharaNames.Count; i++)
            {
                if (dialogueHolder.CharaNames[i].Contains(currEmotion[0]))
                {
                    nameText.text = dialogueHolder.CharaNames[i];
                    imgSprite.sprite = dialogueHolder.FindExpression(nameText.text, currEmotion[1]);
                    Debug.Log(imgSprite.sprite);
                }
            }
        }
        else if (currSentence.Contains("*"))
        {
            nameText.gameObject.SetActive(false);
            imgSprite.gameObject.SetActive(false);
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence());
        
    }

    #region Helper Methods
    // Enqueues the sentences to display
    private void EnqueueSentences(List<string> sent)
    {
        // puts each sentence into the queue
        foreach (string s in sent)
        {
            sentences.Enqueue(s);
        }
    }

    // Starts the animation for the dialogue
    private void StartDialogueAnimation(bool isAutoDialogue)
    {
        animator.SetBool("IsOpen", true);
        isSpeaking = true;
        autoDia = isAutoDialogue;
    }

    // Get the sentences
    private void GetDialogue()
    {
        // clears the queue of sentences if there are any
        if (sentences != null)
        {
            sentences.Clear();
        }

        // clears the dictionary for the new chara sfx to be loaded
        if (charasInDia != null)
        {
            charasInDia.Clear();
        }

        imgSprite.sprite = dialogueHolder.DefaultSprite; // used to set the default sprite to Yuichi's neutral face
        nameText.text = dialogueHolder.Name;

        // add the sentences to the queue, if it isn't during the battle and if there's dialogue typed into the sentences field
        if (!startBattle && dialogueHolder.sentences.Count > 0)
        {
            EnqueueSentences(dialogueHolder.sentences);
        }
        else
        {
            EnqueueSentences(dialogueHolder.CharaLines);
        }

        // checks if there's a dialogue file or just Yuichi's thoughts typed in the public field
        if (dialogueHolder.diaFile != null)
        {
            // get all the sfx for the characters in the dialogue exchange
            for (int i = 0; i < dialogueHolder.CharaNames.Count; i++)
            {
                charasInDia.Add(dialogueHolder.CharaNames[i], new List<AudioClip>()); // initialize the list for the character

                GetSfxForDialogue(dialogueHolder.CharaNames[i]);
            }
        }
        else
        {
            charasInDia.Add(dialogueHolder.Name, new List<AudioClip>()); // initialize the list for Yuichi
            GetSfxForDialogue(dialogueHolder.Name);
        }
    }

    // searches the charaSpeech for the sfx of the characters
    private void GetSfxForDialogue(string characterName)
    {
        for (int i = 0; i < charaSpeech.Count; i++)
        {
            if (charaSpeech[i].name.Contains(characterName))
            {
                charasInDia[characterName].Add(charaSpeech[i]);
            }
        }
    }

    //Completes the sentence ahead of the typing for impatient players
    public void CompleteSentenceDisplay()
    {
        StopAllCoroutines();
        dialogueText.text = currSentence;
        isTyping = false;
    }
    #endregion

    // animates sentences onto the UI
    IEnumerator TypeSentence ()
    {
        dialogueText.text = "";
        isTyping = true;
        List<char> letters = new List<char>(currSentence.ToCharArray());
        float timer = 0.0f; // time between each letter

        // change to while loop so it can have time to play the sfx
        while (letters.Count > 0)
        {
            if (timer >= typingSpeed)
            {
                timer = 0.0f; // resets the timer

                // checks if what is being typed a letter or not
                if (Char.IsLetterOrDigit(letters[0]))
                {
                    // uses the current speaker to randomly play the speaking sfx 
                    /*UnityEngine. -> specifies which namespace is used for random*/
                    audSrc.clip = charasInDia[nameText.text][UnityEngine.Random.Range(0, charasInDia[nameText.text].Count - 1)];
                    audSrc.Play();
                }

                dialogueText.text += letters[0];
                letters.RemoveAt(0);                
            }

            timer += 0.1f;

            yield return null;            
        }

        isTyping = false;

        // if it's autoplaying dialogue, let it display for a few seconds before going away
        if (autoDia)
        {
            StartCoroutine(AutoPlayDialogue());
        }
    }

    // displays the dialogue for a certain amount of time after it's done typing
    IEnumerator AutoPlayDialogue()
    {
        // display on the screen for a few seconds before going to next dialogue
        float duration = 2f;
        int arrayLength = currSentence.ToCharArray().Length;
        

        while (duration > 0f)
        {
            duration -= Time.deltaTime;

            if (duration < 1f)
            {
                DisplayNextSentence();
            }

            yield return null;
        }
    }

    private void EndDialogue()
    {
        isSpeaking = false;
        diaPrompt.SetActive(true);
        animator.SetBool("IsOpen", false);

        if (gObj != null)
        {
            if (startBattle)
            {
                FindObjectOfType<Scenes>().ToBattle(SceneManager.GetActiveScene().name, GameObject.Find("Yuichi").transform.position, gObj.name);
            }
        }
    }
}
