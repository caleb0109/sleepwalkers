using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

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
        Cutscene
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
                FindObjectOfType<NotificationManager>().NotifyInteractUpdate(this);
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
                        FindObjectOfType<NotificationManager>().NotifyInteractUpdate(this);
                    }

                    // if there's a sprite for after interaction, then change the current sprite
                    if (afterInteract != null)
                    {
                        spriteRender.sprite = afterInteract;
                    }

                    interacted = true;
                }
                else if (dia.dialogue.sentences.Count > 0) // only changes the line once
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

            default:
                break;
        }
        //customEvent.Invoke();
    }    
}
