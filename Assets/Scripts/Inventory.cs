using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<GameObject> items = new List<GameObject>();

    // hold the sprites for the items without needing to getComponent each time and overwrite the SEARCH items
    private List<Sprite> itemSprites = new List<Sprite>();

    private Button[] promptBttns = new Button[2];

    public GameObject prompt;

    public GameObject ui_Window;
    public Image[] item_images;

    public GameObject ui_description;
    public Image descriptionImage;
    public Text descriptionText;
    public Text itemTitle;

    [HideInInspector]
    public bool isOpen;

    public void PickUp(GameObject item)
    {
        items.Add(item);
        itemSprites.Add(item.GetComponent<SpriteRenderer>().sprite);

        Update_UI();
    }

    void Update_UI()
    {
        //HideAll();

        for(int i = 0; i < item_images.Length; i++)
        {
            if (i < items.Count && items.Count > 0)
            {
                item_images[i].sprite = itemSprites[i];
                item_images[i].gameObject.SetActive(true);
            } 
            else
            {
                item_images[i].gameObject.SetActive(false);
            }
        }
    }

    void HideAll()
    {
        foreach(var i in item_images)
        {
            i.gameObject.SetActive(false);
        }
    }

    void InvenOn()
    {
        if (FindObjectOfType<Detect>().isExamining)
        {
            return;
        }
        isOpen = !isOpen;
        ui_Window.SetActive(isOpen);
    }

    public void ShowPrompt(int id)
    {
        HideDescription();

        if (items.Count > 0 && id < items.Count)
        {
            prompt.transform.position = item_images[id].gameObject.transform.position;
            prompt.SetActive(true);

            descriptionImage.sprite = item_images[id].sprite;
            descriptionText.text = items[id].GetComponent<Interactable>().descriptionText;
            itemTitle.text = items[id].GetComponent<Interactable>().itemName;

            // sets the prompt buttons once
            if (promptBttns[0] == null)
            {
                for (int i = 0; i < prompt.transform.childCount; i++)
                {
                    promptBttns[i] = prompt.transform.GetChild(i).GetComponent<Button>();
                }

                promptBttns[1].onClick.AddListener(DisplayItemInfo);
            }

            // remove any previous listeners and add a new one so the list isn't giant
            promptBttns[0].onClick.RemoveAllListeners();
            promptBttns[0].onClick.AddListener(() => UseItem(items[id], id));
        }
    }

    public void DisplayItemInfo()
    {
        ui_description.gameObject.SetActive(true);
        HidePrompt();
    }

    public void HideDescription()
    {
        ui_description.gameObject.SetActive(false);
    }

    void OnOpenInven()
    {
        InvenOn();
    }

    public void HidePrompt()
    {
        prompt.SetActive(false);
    }

    // look for a specific item that goes with the requirement
    public void UseItem(GameObject reqItem, int listLoc)
    {
        Interactable item = reqItem.GetComponent<Interactable>();
        Movement playerMove = FindObjectOfType<Movement>();
        Detect detection = FindObjectOfType<Detect>();

        Dialogue use = new Dialogue();
        use.sprite = item.GetComponent<DialogueTrigger>().dialogue.sprite;

        HidePrompt();

        // if the item is useable or food
        if (item.itemType == Interactable.Item.Useable || item.itemType == Interactable.Item.Placeable)
        {
            // if in the correct area, remove item and use it
            if (detection.CheckCorrectArea(item))
            {
                item.notifType = Interactable.NotificationType.removed;

                FindObjectOfType<NotificationManager>().NotifyInteractUpdate(item);

                // remove from inventory
                items.Remove(reqItem);
                itemSprites.RemoveAt(listLoc);

                Update_UI();

                playerMove.OnOpenPhone(); // closes phone window once item is used

                // put the item at the player's current position
                if (item.itemType == Interactable.Item.Placeable)
                {
                    item.transform.position = new Vector3(playerMove.transform.position.x, playerMove.transform.position.y + 1f, 0);
                    item.gameObject.SetActive(true);
                    item.itemType = Interactable.Item.None;
                    item.interactType = Interactable.InteractableType.Cutscene;

                    item.gameObject.GetComponent<DialogueTrigger>().enabled = false;
                }
            }
            else
            {
                // turn off phone and mention that player can't use it in the certain area
                playerMove.OnOpenPhone();

                use.sentences = new List<string>() { "Can't use the " + item.itemName + " here." };
                FindObjectOfType<DialogueManager>().StartDialogue(use, false, null);
            }
        }
        else
        {
            // turn off phone and mention that player can't use it in the certain area
            playerMove.OnOpenPhone();
            use.sentences = new List<string>() { "I rather save this for when I really need it." };
            FindObjectOfType<DialogueManager>().StartDialogue(use, false, null);
        }
    }

    // used for getting items from SEARCHING
    public void CollectItem(GameObject itemHost, Sprite itemSprite)
    {
        itemHost.GetComponent<SpriteRenderer>().sprite = itemSprite; // changes the sprite in the inventory view
        items.Add(itemHost);
        itemSprites.Add(itemSprite);
        Update_UI();
    }
}
