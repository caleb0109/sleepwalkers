using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool startsBattle;

    private void Start()
    {
        dialogue.Start();
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, startsBattle);
    }

    public void TriggerSentence()
    {
        GameObject.Find("BattleManager").GetComponent<DialogueManager>().StartBattleDialogue(dialogue);
    }
}
