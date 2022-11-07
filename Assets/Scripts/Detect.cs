using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Detect : MonoBehaviour
{
    private GameObject detectedObj;
    private SpriteRenderer sprite;
    private Color startColor;

    private DialogueManager dManager;

    //Examine Window
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public Text examineTitle;
    public bool isExamining;

    // Start is called before the first frame update
    public GameObject DetectedObj { get { return detectedObj; } }

    void Start()
    {
        dManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        detectedObj = collision.gameObject;
        //startColor = detectedObj.color;
        //detectedObj.color = Color.yellow;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //detectedObj.color = startColor;
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
        if (detectedObj != null)
        {
            Interactable useArea = detectedObj.GetComponent<Interactable>();

            if (useArea.reqItemName == i.itemName)
            {
                return true;
            }
        }

        return false;
    }

    void OnInteract()
    {
        // used for interaction
        if (detectedObj != null && !dManager.isSpeaking)
        {
            detectedObj.GetComponent<Interactable>().Interact();
        }
        else if (dManager.isSpeaking && !dManager.autoDia) // if there is nothing to interact with
        {
            if (dManager.isTyping)
            {
                dManager.CompleteSentenceDisplay();
            }
            else
            {
                dManager.DisplayNextSentence();
            }
        }
    }
}
