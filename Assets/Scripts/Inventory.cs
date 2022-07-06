using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{


    // Start is called before the first frame update
    private List<GameObject> items = new List<GameObject>();

    public bool isOpen;

    public GameObject ui_Window;
    public Image[] item_images;

    public GameObject ui_description;
    public Image descriptionImage;
    public Text descriptionText;

    public void PickUp(GameObject item)
    {
        items.Add(item);
        Update_UI();
    }

    void Update_UI()
    {
        HideAll();

        for(int i = 0; i < items.Count; i++)
        {
            item_images[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
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

    //need to figure this part out later
    public void InvenDescription(int id)
    {
        descriptionImage.sprite = item_images[id].sprite;
        descriptionText.text = items[id].GetComponent<Interactable>().descriptionText;

        ui_description.gameObject.SetActive(true);
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
    public void SearchInventory(GameObject reqItem)
    {

    }
}
