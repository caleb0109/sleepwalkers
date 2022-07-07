using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<GameObject> items = new List<GameObject>();

    // hold the sprites for the items without needing to getComponent each time and overwrite the SEARCH items
    private List<Sprite> itemSprites = new List<Sprite>(); 

    public bool isOpen;

    public GameObject ui_Window;
    public Image[] item_images;

    public GameObject ui_description;
    public Image descriptionImage;
    public Text descriptionText;
    public Text itemTitle;

    public void PickUp(GameObject item)
    {
        items.Add(item);
        itemSprites.Add(item.GetComponent<SpriteRenderer>().sprite);


        Update_UI();
    }

    void Update_UI()
    {
        //HideAll();

        for(int i = 0; i < items.Count; i++)
        {
            item_images[i].sprite = itemSprites[i];
            item_images[i].gameObject.SetActive(true);
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

    public void DisplayItemInfo(int id)
    {
        if (items.Count > 0)
        {
            descriptionImage.sprite = item_images[id].sprite;
            descriptionText.text = items[id].GetComponent<Interactable>().descriptionText;
            itemTitle.text = items[id].GetComponent<Interactable>().itemName;

            ui_description.gameObject.SetActive(true);
        }
    }

    public void HideDescription()
    {
        ui_description.gameObject.SetActive(false);
    }

    void OnOpenInven()
    {
        InvenOn();
    }

    // look for a specific item that goes with the requirement
    public void UseItem(GameObject reqItem)
    {

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
