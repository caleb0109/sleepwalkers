using System.Collections;
using System.Collections.Generic;
//using UnityEngine.Events;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public enum ItemType
    {
        NONE,
        PickUp,
        Examine
    }
    public ItemType type;

    public string descriptionText;

    //public UnityEvent customEvent;

    public void Interact()
    {
        switch(type)
        {
            case ItemType.PickUp:
                FindObjectOfType<Inventory>().PickUp(gameObject);
                gameObject.SetActive(false);
                break;

            case ItemType.Examine:
                if (FindObjectOfType<Inventory>().isOpen)
                {
                    break;
                }
                FindObjectOfType<Detect>().ExamineItem(this);
                break;

            default:
                break;
        }
        //customEvent.Invoke();
    }

    
}
