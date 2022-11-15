// Base code used from https://www.youtube.com/watch?v=_nRzoTzeyxU 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class DialogueManager : MonoBehaviour
{
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
    private bool startBattle;
    private GameObject gObj;
    //private float multiplier; // used in the future for if the user wants to change the speed of the autoplay
    private string currSentence;

    void Start()
    {
        sentences = new Queue<string>();
        isSpeaking = false;
        isTyping = false;
        startBattle = false;
        //multiplier = 1f;
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
            for(int i = 0; i < dialogueHolder.CharaNames.Count; i++)
            {
                if (dialogueHolder.CharaNames[i].Contains(currEmotion[0]))
                {
                    nameText.text = dialogueHolder.CharaNames[i];
                    imgSprite.sprite = dialogueHolder.FindExpression(nameText.text, currEmotion[1]);
                }
            }
        }
        else if (currSentence.Contains("*"))
        {
            nameText.gameObject.SetActive(false);
            imgSprite.gameObject.SetActive(false);
        }

        StopAllCoroutines();
        if (autoDia)
        {
            StartCoroutine(AutoPlayDialogue());
        }
        StartCoroutine(TypeSentence());
        
    }

    #region Helper Methods
    // Enqueues the sentences
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

        imgSprite.sprite = dialogueHolder.DefaultSprite; // used to set the default sprite to Yuichi's neutral face
        nameText.text = dialogueHolder.Name;

        if (!startBattle && dialogueHolder.sentences.Count > 0)
        {
            EnqueueSentences(dialogueHolder.sentences);
        }
        else
        {
            EnqueueSentences(dialogueHolder.CharaLines);
        }
    }

    //Completes the sentence ahead of the typing for impatient players
    public void CompleteSentenceDisplay()
    {
        dialogueText.text = currSentence;
        isTyping = false;
    }
    #endregion

    // animates sentences onto the UI
    IEnumerator TypeSentence ()
    {
        dialogueText.text = "";
        isTyping = true;
        char[] letters = currSentence.ToCharArray();
        for (int i = 0; i < letters.Length; i++)
        {
            char l = letters[i];
            dialogueText.text += l;

            if (i + 1 == letters.Length)
            {
                isTyping = false;
            }

            yield return null;            
        }
    }

    // displays the dialogue for a certain amount of time
    IEnumerator AutoPlayDialogue()
    {
        // display on the screen for a few seconds before going to next dialogue
        float duration;
        int arrayLength = currSentence.ToCharArray().Length;

        // determines how long the dialogue should be displayed based on the string length
        if (arrayLength > 300)
        {
            duration = 10f;
        }
        else if (arrayLength > 100)
        {
            duration = 6f;
        }
        else if (arrayLength > 50)
        {
            duration = 5f;
        }
        else if (arrayLength > 25)
        {
            duration = 4f;
        }
        else
        {
            duration = 3f;
        }

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
            PlayableDirector pDirect = gObj.GetComponent<PlayableDirector>();

            if (pDirect)
            {
                pDirect.enabled = false;
            }

            if (startBattle)
            {
                FindObjectOfType<Scenes>().ToBattle(SceneManager.GetActiveScene().name, GameObject.Find("Yuichi").transform.position, gObj.name);
            }
        }
    }
}
