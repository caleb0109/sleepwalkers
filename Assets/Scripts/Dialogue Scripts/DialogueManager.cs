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
    public bool isSpeaking;

    private Queue<string> sentences;
    private Dialogue dialogueHolder;

    void Start()
    {
        sentences = new Queue<string>();
        isSpeaking = false;
    }

    public void StartDialogue(Dialogue dia)
    {
        dialogueHolder = dia;

        animator.SetBool("IsOpen", true);
        isSpeaking = true;

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
            nameText.text = dia.name;
            imgSprite.sprite = dia.sprite;

            // puts each sentence into the queue
            foreach (string sent in dia.sentences)
            {
                sentences.Enqueue(sent);
            }
        }

        DisplayNextSentence(); // starts the first sentence
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

    private void EndDialogue()
    {
        isSpeaking = false;
        animator.SetBool("IsOpen", false);
    }
}
