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
        Trigger,
        Cutscene,
        SwitchScene,
        Cafeteria
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
    //private int interactCount;

    private Scenes sManager;
    private Inventory inventManager;
    private NotificationManager notifManager;

    private void Start()
    {
        spriteRender = this.gameObject.GetComponent<SpriteRenderer>();
        interacted = false;
        alreadyInteracted = "I already searched that.";
        dia = this.gameObject.GetComponent<DialogueTrigger>();
        //interactCount = 0;
        sManager = FindObjectOfType<Scenes>();
        inventManager = FindObjectOfType<Inventory>();
        notifManager = FindObjectOfType<NotificationManager>();
    }

    public void Interact()
    {
        switch (interactType)
        {
            case InteractableType.PickUp:
                inventManager.PickUp(gameObject);
                notifManager.NotifyInteractUpdate(this);
                gameObject.SetActive(false);
                if (highlight)
                {
                    highlight.SetActive(false);
                }

                dia.TriggerDialogue(); // start dialogue

                break;

            case InteractableType.Examine:
                /*if (inventManager.isOpen)
                {
                    break;
                }*/
                FindObjectOfType<Detect>().ExamineItem(this);
                break;

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
                        inventManager.CollectItem(gameObject, searchItemSprite);
                        notifManager.NotifyInteractUpdate(this);
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
                    GameObject.Find("Yuichi").GetComponent<SpriteRenderer>().enabled = false;
                }
                break;

            case InteractableType.SwitchScene:
                //GameObject.Find("GameManager").GetComponent<SaveManager>().SaveGame("autoSave");

                // TODO: figure out a non hard cody way for this case
                if (this.gameObject.transform.parent != null && this.gameObject.transform.parent.name.Contains("library"))
                {
                    sManager.GoToSpecificScene("Library");
                }
                else if (this.gameObject.transform.parent != null && this.gameObject.transform.parent.name.Contains("cafeteria"))
                {
                    sManager.GoToSpecificScene("Cafeteria");
                }
                else if (this.gameObject.transform.parent != null && this.gameObject.transform.parent.name.Contains("classroom"))
                {
                    sManager.GoToSpecificScene("Classroom");
                }
                else
                {
                    sManager.GoToSpecificScene("Dream School");
                }
                break;

            case InteractableType.Cafeteria:

                Debug.Log("Interacting with: " + this.gameObject);

                // if the player has the order, place it down
                CafeteriaMinigame cafeMini = FindObjectOfType<CafeteriaMinigame>();
                if (cafeMini.itemInHand != null)
                {
                    cafeMini.PlaceOrder(this.gameObject);
                }
                else if (!this.name.Contains("student"))
                {
                    cafeMini.PickUpOrder(this.gameObject);
                }
                break;

            default:
                break;
        }
    }  
}
