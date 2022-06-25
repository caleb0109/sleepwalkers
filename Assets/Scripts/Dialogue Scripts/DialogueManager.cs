// Base code used from https://www.youtube.com/watch?v=_nRzoTzeyxU 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Image npcImage;
    public Animator animator;
    public bool isSpeaking;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        isSpeaking = false;
    }

    public void StartDialogue(Dialogue dia)
    {
        animator.SetBool("IsOpen", true);

        isSpeaking = true;
        nameText.text = dia.name;
        npcImage.sprite = dia.npcSprite;
        
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
