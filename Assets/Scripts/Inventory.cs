using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<GameObject> items = new List<GameObject>();

    private InventoryUI invenUI;

    [HideInInspector]
    public bool isOpen;
    public GameObject equippedWeapon = null;

    public List<GameObject> Items { get { return items; } }

    void Start()
    {
        invenUI = this.gameObject.GetComponent<InventoryUI>();
    }

    public void PickUp(GameObject item)
    {
        items.Add(item);
        invenUI.AddItemSprite(item.GetComponent<SpriteRenderer>().sprite);

        invenUI.Update_UI();
    }

    protected void InvenOn()
    {
        if (FindObjectOfType<Detect>().isExamining)
        {
            return;
        }
        isOpen = !isOpen;
    }

    void OnOpenInven()
    {
        InvenOn();
    }

    // used for getting items from SEARCHING
    public void CollectItem(GameObject itemHost, Sprite itemSprite)
    {
        itemHost.GetComponent<SpriteRenderer>().sprite = itemSprite; // changes the sprite in the inventory view
        items.Add(itemHost);
        invenUI.AddItemSprite(itemSprite);
        invenUI.Update_UI();
    }

    // equip weapon
    public void EquipWeapon(GameObject weapon, int listLoc)
    {
        invenUI.HidePrompt();

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
        FindObjectOfType<Phone>().TogglePhone();

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
        FindObjectOfType<Phone>().TogglePhone();

        equippedWeapon = null;
    }

    // used to move the item, set it active, and turn off the collider for the detectedObj
    public void PlaceItem(GameObject item, Detect collidedObj)
    {
        FindObjectOfType<Nodes>().MoveItemToNode(item); // move the item
        collidedObj.DetectedObj.GetComponent<BoxCollider2D>().enabled = false; // turn off the collider for the specifc item loc
        item.SetActive(true);
    }

    public void RemoveItem(GameObject itemToRemove)
    {
        items.Remove(itemToRemove);
    }

    // checks if the player has the required item to procceed
    public bool CheckInventory(GameObject requirement)
    {
        foreach (GameObject g in items)
        {
            if (g == requirement)
            {
                return true;
            }
        }

        return false;
    }
}
