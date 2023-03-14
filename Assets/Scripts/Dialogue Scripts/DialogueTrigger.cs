using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool startsBattle;
    public TextAsset afterSpeak;

    private int interactCount;

    [HideInInspector] public GameObject objTrigger;
    private void Start()
    {
        dialogue.Start();
        objTrigger = this.gameObject;
        interactCount = 0;
    }

    public void TriggerDialogue()
    {
        //Debug.Log(this.gameObject.name + " " + interactCount);
        if (afterSpeak != null)
        {
            if (interactCount == 1)
            {
                dialogue.diaFile = afterSpeak;
                dialogue.ResetAndLoadNewDialogue();
            }
            else
            {
                interactCount++;
            }
        }

        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, startsBattle, objTrigger);
    }

    public void TriggerSentence()
    {
        GameObject.Find("BattleManager").GetComponent<DialogueManager>().StartBattleDialogue(dialogue);
    }

    public void TriggerAutoDia()
    {
        FindObjectOfType<DialogueManager>().StartAutoDialogue(dialogue);
    }
}
