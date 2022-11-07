using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
/*
 * CODE WRITTEN AND DEVELOPED BY POM POM PUDDING G AND YESSICA !!!! thank you takumi
 * 
 * */
public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update
    public enum InteractableType
    {
        NONE,
        PickUp,
        Examine,
        Search,
        Npc,
        Talking, // used for Yuichi to talk to himself
        Cutscene,
        SideQuest
    }

    public enum NotificationType
    {
        none,
        item,
        article,
        task,
        photo,
        removed, // used for placing items, or removed items in general
        equipped,
        unequipped
    }

    public enum Item
    {
        None,
        Placeable,
        Useable,
        Weapon,
        Food
    }

    public string itemName;

    public string reqItemName;

    public Sprite searchItemSprite; // item sprite that goes with the search

    public InteractableType interactType;
    public NotificationType notifType;
    public Item itemType;

    public string descriptionText;

    public Sprite afterInteract;
    public GameObject highlight;

    private SpriteRenderer spriteRender; // used to change sprites for InteractableType.Search
    private bool interacted;
    private string alreadyInteracted;

    private DialogueTrigger dia;
    private DialogueManager dManager;
    private int interactCount;

    //public UnityEvent customEvent;

    private void Start()
    {
        spriteRender = this.gameObject.GetComponent<SpriteRenderer>();
        interacted = false;
        alreadyInteracted = "I already searched that.";
        dia = this.gameObject.GetComponent<DialogueTrigger>();
        dManager = FindObjectOfType<DialogueManager>();
        interactCount = 0;
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractableType.PickUp:
                FindObjectOfType<Inventory>().PickUp(gameObject);
                FindObjectOfType<NotificationManager>().NotifyInteractUpdate(this);
                gameObject.SetActive(false);
                if (highlight)
                {
                    highlight.SetActive(false);
                }

                dia.TriggerDialogue(); // start dialogue

                break;

            case InteractableType.Examine:
                if (FindObjectOfType<Inventory>().isOpen)
                {
                    break;
                }
                FindObjectOfType<Detect>().ExamineItem(this);
                break;

            case InteractableType.Talking:
            case InteractableType.Npc:
                if (highlight)
                {
                    highlight.SetActive(false);
                }
                dia.TriggerDialogue(); // start dialogue          
                break;

            case InteractableType.Search:

                if (!interacted) // prevents player from continuiously searching the same thing
                {
                    // if it contains an item, then add item to inventory
                    if (itemName != "")
                    {
                        FindObjectOfType<Inventory>().CollectItem(gameObject, searchItemSprite);
                        FindObjectOfType<NotificationManager>().NotifyInteractUpdate(this);
                    }

                    // if there's a sprite for after interaction, then change the current sprite
                    if (afterInteract != null)
                    {
                        spriteRender.sprite = afterInteract;
                        if (highlight)
                        {
                            highlight.SetActive(false);
                        }
                    }

                    interacted = true;
                }
                else if (dia.dialogue.sentences[0] != alreadyInteracted) // only changes the line once
                {
                    dia.dialogue.sentences = new List<string>() { alreadyInteracted };
                }

                dia.TriggerDialogue(); // say something about the search

                break;

            case InteractableType.Cutscene:

                PlayableDirector p = this.gameObject.GetComponent<PlayableDirector>();

                if (p.isActiveAndEnabled)
                {
                    p.Play(); // play the cutscene
                    GameObject.Find("Yuichi").SetActive(false);
                }
                break;

            case InteractableType.SideQuest:
                


                dia.TriggerDialogue(); // start dialogue
                interactCount++;
                break;

            default:
                break;
        }
        //customEvent.Invoke();
    }  
}
