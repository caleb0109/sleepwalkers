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

    void Start()
    {
        sentences = new Queue<string>();
        isSpeaking = false;
        startBattle = false;
    }

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
            foreach (string sent in dia.CharaLines)
            {
                sentences.Enqueue(sent);
            }
        }
        else
        {
            nameText.text = dia.Name;
            imgSprite.sprite = dia.sprite;

            // puts each sentence into the queue
            foreach (string sent in dia.sentences)
            {
                sentences.Enqueue(sent);
            }
        }

        DisplayNextSentence(); // starts the first sentence
    }

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
            foreach (string sent in dia.CharaLines)
            {
                Debug.Log(sent);
                sentences.Enqueue(sent);
            }
        }
        else
        {
            nameText.text = dia.Name;
            imgSprite.sprite = dia.sprite;

            // puts each sentence into the queue
            foreach (string sent in dia.sentences)
            {
                sentences.Enqueue(sent);
            }
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
                    imgSprite.sprite = dialogueHolder.charaSprites[i];
                    break;
                }
            }
        }

        StopAllCoroutines();
        if (autoDia)
        {
            StartCoroutine(AutoPlayDialogue());
        }
        StartCoroutine(TypeSentence(sentence));
        
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


    IEnumerator AutoPlayDialogue()
    {
        // display on the screen for 4 sec before starting next dialogue
        float duration = 4f; //TODO: find a way to change duration according to length of the line

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
