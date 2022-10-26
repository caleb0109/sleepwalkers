using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool startsBattle;

    [HideInInspector] public GameObject objTrigger;
    private void Start()
    {
        dialogue.Start();
        objTrigger = this.gameObject;
    }

    public void TriggerDialogue()
    {
        Debug.Log(objTrigger);
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
