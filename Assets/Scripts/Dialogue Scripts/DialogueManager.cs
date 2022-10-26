// Base code used from https://www.youtube.com/watch?v=_nRzoTzeyxU 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Image imgSprite;
    public Animator animator;
    public GameObject diaPrompt;
    public bool isSpeaking;

    private Queue<string> sentences;
    private Dialogue dialogueHolder;
    private bool startBattle;
    private GameObject gObj;
    private bool autoDia;
    private float multiplier; // used in the future for if the user wants to change the speed of the autoplay
    private char[] sentLength; // determines the num of characters in the string array

    void Start()
    {
        sentences = new Queue<string>();
        isSpeaking = false;
        startBattle = false;
        multiplier = 1f;
    }

    // used for normal dialogue between characters or with Yuichi
    public void StartDialogue(Dialogue dia, bool battle, GameObject obj)
    {
        dialogueHolder = dia;
        startBattle = battle;
        gObj = obj;

        animator.SetBool("IsOpen", true);
        isSpeaking = true;
        autoDia = false;

        // clears the queue of sentences if there are any
        if (sentences != null)
        {
            sentences.Clear();
        }

        // used to differentiate between multi character and single character dialogue
        if (dia.diaFile)
        {
            EnqueueSentences(dia.CharaLines);
        }
        else
        {
            nameText.text = dia.Name;
            imgSprite.sprite = dia.Sprite;

            // puts each sentence into the queue
            EnqueueSentences(dia.sentences);
        }

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

        string sentence = sentences.Dequeue();

        if (sentence.Contains("|"))
        {
            string[] split = sentence.Split('|');
            sentence = split[1];
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    // AutoPlays Dialogue
    public void StartAutoDialogue(Dialogue dia)
    {
        dialogueHolder = dia;
        animator.SetBool("IsOpen", true);
        autoDia = true;
        diaPrompt.SetActive(false);
        isSpeaking = true;

        // clears the queue of sentences if there are any
        if (sentences != null)
        {
            sentences.Clear();
        }

        // puts each sentence into the queue
        if (dia.diaFile)
        {
            EnqueueSentences(dia.CharaLines);
        }
        else
        {
            nameText.text = dia.Name;
            imgSprite.sprite = dia.Sprite;

            // puts each sentence into the queue
            EnqueueSentences(dia.sentences);
        }

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
        string sentence = sentences.Dequeue();
        
        if (sentence.Contains("|"))
        {
            string[] split = sentence.Split('|');
            sentence = split[1];

            // look for the name and set the correct sprite and name for the line
            for(int i = 0; i < dialogueHolder.CharaNames.Count; i++)
            {
                if (dialogueHolder.CharaNames[i].Contains(split[0]))
                {
                    nameText.text = dialogueHolder.CharaNames[i];
                    imgSprite.sprite = dialogueHolder.CharaSprites[i];
                    break;
                }
            }
        }

        sentLength = sentence.ToCharArray();

        StopAllCoroutines();
        if (autoDia)
        {
            StartCoroutine(AutoPlayDialogue());
        }
        StartCoroutine(TypeSentence(sentence));
        
    }

    // helper method to help enqueue the sentences
    private void EnqueueSentences(List<string> sent)
    {
        // puts each sentence into the queue
        foreach (string s in sent)
        {
            sentences.Enqueue(s);
        }
    }

    // animates sentences onto the UI
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    // displays the dialogue for a certain amount of time
    IEnumerator AutoPlayDialogue()
    {
        // display on the screen for a few seconds before going to next dialogue
        float duration;
        int arrayLength = sentLength.Length;

        // determines how long the dialogue should be displayed based on the string length
        if (arrayLength > 300)
        {
            duration = 8f;
        }
        else if (arrayLength > 100)
        {
            duration = 5f;
        }
        else if (arrayLength > 50)
        {
            duration = 4f;
        }
        else if (arrayLength > 25)
        {
            duration = 3.5f;
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

        if (startBattle)
        {
            FindObjectOfType<Scenes>().ToBattle("Alleyway", GameObject.Find("Yuichi").transform.position, gObj);
        }
    }
}
