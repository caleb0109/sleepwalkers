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
        Npc
    }

    public enum NotificationType
    {
        item,
        article,
        task,
        none
    }

    public string itemName;

    public InteractableType interactType;

    public NotificationType notifType;

    public string descriptionText;

    public Sprite afterInteract;

    private SpriteRenderer spriteRender; // used to change sprites for InteractableType.Search
    private bool interacted;
    private string alreadyInteracted;

    //public UnityEvent customEvent;

    private void Start()
    {
        spriteRender = this.gameObject.GetComponent<SpriteRenderer>();
        interacted = false;
        alreadyInteracted = "I already searched that.";
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractableType.PickUp:
                FindObjectOfType<Inventory>().PickUp(gameObject);
                FindObjectOfType<NotificationManager>().NotifyUpdates(this);
                gameObject.SetActive(false);
                break;

            case InteractableType.Examine:
                if (FindObjectOfType<Inventory>().isOpen)
                {
                    break;
                }
                FindObjectOfType<Detect>().ExamineItem(this);
                break;

            case InteractableType.Npc:
                this.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue(); // used to get this specific trigger only
                break;

            case InteractableType.Search:

                if (!interacted) // prevents player from continuiously searching the same thing
                {
                    // if it contains an item, then add item to inventory
                    if (itemName != "")
                    {
                        FindObjectOfType<Inventory>().PickUp(gameObject);
                        FindObjectOfType<NotificationManager>().NotifyUpdates(this);
                    }

                    // if there's a sprite for after interaction, then change the current sprite
                    if (afterInteract != null)
                    {
                        spriteRender.sprite = afterInteract;
                    }

                    interacted = true;
                }
                else if (this.gameObject.GetComponent<DialogueTrigger>().dialogue.sentences.Length > 0) // only changes the line once
                {
                    this.gameObject.GetComponent<DialogueTrigger>().dialogue.sentences = new string[] { alreadyInteracted };
                }

                this.gameObject.GetComponent<DialogueTrigger>().TriggerDialogue(); // say something about the search

                break;

            default:
                break;
        }
        //customEvent.Invoke();
    }

    
}
