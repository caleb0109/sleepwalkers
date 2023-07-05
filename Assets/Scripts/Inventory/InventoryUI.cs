using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Button[] promptBttns = new Button[2];
    private Inventory invent;

    // hold the sprites for the items without needing to getComponent each time and overwrite the SEARCH items
    private List<Sprite> itemSprites = new List<Sprite>();

    public GameObject prompt;

    public GameObject ui_Window;
    public Image[] item_images;

    public GameObject ui_description;
    public Image descriptionImage;
    public Text descriptionText;
    public Text itemTitle;

    void Start()
    {
        invent = this.gameObject.GetComponent<Inventory>();
    }

    // adds the Item Sprite to inventory
    public void AddItemSprite(Sprite itemSprite)
    {
        itemSprites.Add(itemSprite);
    }

    public void ShowPrompt(int id)
    {
        HideDescription();

        if (invent.Items.Count > 0 && id < invent.Items.Count)
        {
            prompt.transform.position = item_images[id].gameObject.transform.position;
            prompt.SetActive(true);

            descriptionImage.sprite = item_images[id].sprite;
            descriptionText.text = invent.Items[id].GetComponent<Interactable>().descriptionText;
            itemTitle.text = invent.Items[id].GetComponent<Interactable>().itemName;

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
            /*if (invent.Items[id].GetComponent<Interactable>().itemType == Interactable.Item.Weapon)
            {
                if (invent.equippedWeapon != null)
                {
                    promptBttns[0].transform.GetChild(0).GetComponent<Text>().text = "Unequip Item";
                    promptBttns[0].onClick.AddListener(invent.UnequipWeapon);
                }
                else
                {
                    promptBttns[0].transform.GetChild(0).GetComponent<Text>().text = "Equip Item";
                    promptBttns[0].onClick.AddListener(() => invent.EquipWeapon(invent.Items[id], id));
                }
            }
            else
            {
                promptBttns[0].transform.GetChild(0).GetComponent<Text>().text = "Use Item";
                promptBttns[0].onClick.AddListener(() => UseItem(invent.Items[id], id));
            }*/
        }
    }

    public void HidePrompt()
    {
        prompt.SetActive(false);
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

    public void Update_UI()
    {
        for (int i = 0; i < item_images.Length; i++)
        {
            if (i < invent.Items.Count && invent.Items.Count > 0)
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

    // look for a specific item that goes with the requirement
    public void UseItem(GameObject reqItem, int listLoc)
    {
        Interactable item = reqItem.GetComponent<Interactable>();
        Phone playerPhone = FindObjectOfType<Phone>();
        Detect detection = FindObjectOfType<Detect>();

        Dialogue use = new Dialogue();
        use.Start();

        HidePrompt();

        // if the item is useable or food
       /*if (item.itemType == Interactable.Item.Useable || item.itemType == Interactable.Item.Placeable)
        {
            Debug.Log("I'm using the item");
            // if in the correct area, remove item and use it
            if (detection.CheckCorrectArea(item))
            {
                item.notifType = Interactable.NotificationType.removed;

                FindObjectOfType<NotificationManager>().NotifyInteractUpdate(item);

                // remove from inventory
                invent.RemoveItem(reqItem);
                itemSprites.RemoveAt(listLoc);

                Update_UI();

                playerPhone.TogglePhone(); // closes phone window once item is used

                // put the item at the node closet to player
                if (item.itemType == Interactable.Item.Placeable)
                {
                    Debug.Log("I'm placing the item");
                    // set the item to active and change the properties
                    invent.PlaceItem(reqItem, detection);
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
                playerPhone.TogglePhone();

                use.sentences = new List<string>() { "Can't use the " + item.itemName + " here." };
                FindObjectOfType<DialogueManager>().StartDialogue(use, false, null);
            }
        }*/
    }
}
