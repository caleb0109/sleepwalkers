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

    public InteractableType type;

    public NotificationType notifType;

    public string descriptionText;

    //public UnityEvent customEvent;

    public void Interact()
    {
        switch(type)
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
                FindObjectOfType<NpcDialogueTrigger>().TriggerDialogue();
                break;

            default:
                break;
        }
        //customEvent.Invoke();
    }

    
}
