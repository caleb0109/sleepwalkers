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
    public GameObject equippedWeapon = null;

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

            // if the item is a weapon, display different option. Else display Use otion
            if (items[id].GetComponent<Interactable>().itemType == Interactable.Item.Weapon)
            {
                if (equippedWeapon != null)
                {
                    promptBttns[0].transform.GetChild(0).GetComponent<Text>().text = "Unequip Item";
                    promptBttns[0].onClick.AddListener(UnequipWeapon);
                }
                else
                {
                    promptBttns[0].transform.GetChild(0).GetComponent<Text>().text = "Equip Item";
                    promptBttns[0].onClick.AddListener(() => EquipWeapon(items[id], id));
                }
            }
            else
            {
                promptBttns[0].transform.GetChild(0).GetComponent<Text>().text = "Use Item";
                promptBttns[0].onClick.AddListener(() => UseItem(items[id], id));
            }

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

    // equip weapon
    public void EquipWeapon(GameObject weapon, int listLoc)
    {
        HidePrompt();

        // if the player equips a weapon while another one is equipped, unequip the prev weapon
        if (equippedWeapon != null)
        {
            UnequipWeapon();
        }

        // notify the player the item's been equipped
        Interactable eWeapon = weapon.GetComponent<Interactable>();
        eWeapon.notifType = Interactable.NotificationType.equipped; // change the notif type
        FindObjectOfType<NotificationManager>().NotifyInteractUpdate(eWeapon);

        equippedWeapon = weapon; // equip the new weapon

        // close phone
        FindObjectOfType<Movement>().OnOpenPhone();

        // say something about getting the weapon equipped
        Dialogue equip = new Dialogue();
        equip.Start();

        equip.sentences = new List<string>() { $"This {weapon.GetComponent<Interactable>().itemName}'ll be helpful for whatever dangers comes my way." };
        FindObjectOfType<DialogueManager>().StartDialogue(equip, false, null);
    }

    public void UnequipWeapon()
    {
        // notify the player the item's been unequipped
        Interactable eWeapon = equippedWeapon.GetComponent<Interactable>();
        eWeapon.notifType = Interactable.NotificationType.unequipped;

        FindObjectOfType<NotificationManager>().NotifyInteractUpdate(eWeapon);

        // close phone
        FindObjectOfType<Movement>().OnOpenPhone();

        equippedWeapon = null;
    }


    // look for a specific item that goes with the requirement
    public void UseItem(GameObject reqItem, int listLoc)
    {
        Interactable item = reqItem.GetComponent<Interactable>();
        Movement playerMove = FindObjectOfType<Movement>();
        Detect detection = FindObjectOfType<Detect>();

        Dialogue use = new Dialogue();
        use.Start();

        HidePrompt();

        // if the item is useable or food
        if (item.itemType == Interactable.Item.Useable || item.itemType == Interactable.Item.Placeable)
        {
            Debug.Log("I'm using the item");
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

                // put the item at the node closet to player
                if (item.itemType == Interactable.Item.Placeable)
                {
                    Debug.Log("I'm placing the item");
                    // set the item to active and change the properties
                    PlaceItem(reqItem, detection);
                    item.itemType = Interactable.Item.None;
                    item.interactType = Interactable.InteractableType.Cutscene;

                    if (item.gameObject.GetComponent<DialogueTrigger>() != null)
                    {
                        item.gameObject.GetComponent<DialogueTrigger>().enabled = false;
                    }
                    //item.gameObject.GetComponent<DialogueTrigger>().enabled = false;
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
    }

    // used for getting items from SEARCHING
    public void CollectItem(GameObject itemHost, Sprite itemSprite)
    {
        itemHost.GetComponent<SpriteRenderer>().sprite = itemSprite; // changes the sprite in the inventory view
        items.Add(itemHost);
        itemSprites.Add(itemSprite);
        Update_UI();
    }

    // used to move the item, set it active, and turn off the collider for the detectedObj
    public void PlaceItem(GameObject item, Detect collidedObj)
    {
        FindObjectOfType<Nodes>().MoveItemToNode(item); // move the item
        collidedObj.DetectedObj.GetComponent<BoxCollider2D>().enabled = false; // turn off the collider for the specifc item loc
        item.SetActive(true);
    }

    // checks if the player has the required item to procceed
    public bool CheckInventory(GameObject requirement)
    {
        SaveInventory();

        foreach (GameObject g in items)
        {
            if (g == requirement)
            {
                return true;
            }
        }

        return false;
    }

    public void SaveInventory()
    {

    }

    public void LoadInventory()
    {

    }

}
