using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Events;
using UnityEngine;

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
        Talking // used for Yuichi to talk to himself
    }

    public enum NotificationType
    {
        item,
        article,
        task,
        removed, // used for placing items, or removed items in general
        none
    }

    public string itemName;

    public Sprite searchItemSprite; // item sprite that goes with the search

    public InteractableType interactType;

    public NotificationType notifType;

    public string descriptionText;

    public Sprite afterInteract;

    private SpriteRenderer spriteRender; // used to change sprites for InteractableType.Search
    private bool interacted;
    private string alreadyInteracted;

    private DialogueTrigger dia;

    //public UnityEvent customEvent;

    private void Start()
    {
        spriteRender = this.gameObject.GetComponent<SpriteRenderer>();
        interacted = false;
        alreadyInteracted = "I already searched that.";
        dia = this.gameObject.GetComponent<DialogueTrigger>();
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractableType.PickUp:
                FindObjectOfType<Inventory>().PickUp(gameObject);
                FindObjectOfType<NotificationManager>().NotifyUpdates(this);
                gameObject.SetActive(false);

                // say something only if there's dialogue attached to this item
                if (dia)
                {
                    dia.TriggerDialogue();
                }
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
                dia.TriggerDialogue(); // used to get this specific trigger only
                break;

            case InteractableType.Search:

                if (!interacted) // prevents player from continuiously searching the same thing
                {
                    // if it contains an item, then add item to inventory
                    if (itemName != "")
                    {
                        FindObjectOfType<Inventory>().CollectItem(gameObject, searchItemSprite);
                        FindObjectOfType<NotificationManager>().NotifyUpdates(this);
                    }

                    // if there's a sprite for after interaction, then change the current sprite
                    if (afterInteract != null)
                    {
                        spriteRender.sprite = afterInteract;
                    }

                    interacted = true;
                }
                else if (dia.dialogue.sentences.Length > 0) // only changes the line once
                {
                    dia.dialogue.sentences = new string[] { alreadyInteracted };
                }

                dia.TriggerDialogue(); // say something about the search

                break;

            default:
                break;
        }
        //customEvent.Invoke();
    }

    
}
