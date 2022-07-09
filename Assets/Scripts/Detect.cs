using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Detect : MonoBehaviour
{
    private GameObject detectedObj;

    //Examine Window
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public Text examineTitle;
    public bool isExamining;

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedObj = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedObj = null;
    }

    public void ExamineItem(Interactable item)
    {
        if(isExamining)
        {
            examineWindow.SetActive(false);
            isExamining = false;
        }
        else
        {
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            examineText.text = item.descriptionText;
            examineTitle.text = item.name;
            examineWindow.SetActive(true);
            isExamining = true;
        }
    }


    // checks if this is where the item needs to be used
    public bool CheckCorrectArea(Interactable i)
    {

        return false;
    }

    void OnInteract()
    {
        if (detectedObj != null)
        {
            detectedObj.GetComponent<Interactable>().Interact();
        }
    }
}
