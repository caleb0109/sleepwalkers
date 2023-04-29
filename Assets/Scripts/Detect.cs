using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Detect : MonoBehaviour
{
    private GameObject detectedObj;
    private SpriteRenderer sprite;
    private Color startColor;

    private DialogueManager dManager;
    private Scenes sManager;

    private int interactionCounter;
    static bool battled;

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
        interactionCounter = 0;
        sManager = FindObjectOfType<Scenes>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedObj = collision.gameObject;
        if (detectedObj.GetComponent<Interactable>() && 
            detectedObj.GetComponent<Interactable>().interactType == Interactable.InteractableType.Trigger)
        {
            
         // get the detectedObj's dialogue trigger
         DialogueTrigger d = detectedObj.GetComponent<DialogueTrigger>();
         
         if (SceneManager.GetActiveScene().name == "Library")
         {
             interactionCounter++;
             // find the dialogue file pertaining to the strike
             d.dialogue.diaFile = Resources.Load<TextAsset>($"Files/Dialogue_Files/{sManager.FindCurrentScene()}/strike{interactionCounter}");
         }

         // call the start to load all the sprites, file, etc
         d.dialogue.Start();
         // trigger the dialogue
         if (interactionCounter == 3)
         {
             d.startsBattle = true;
             battled = true;
         }

         //Debug.Log(battled);
         d.TriggerDialogue();
         
         SoundEffect s = detectedObj.GetComponent<SoundEffect>();
            if (s != null)
            {
                s.PlaySound();
            }

            //detectedObj.SetActive(false);
            if (detectedObj.GetComponent<SpriteRenderer>())
            {
                detectedObj.GetComponent<SpriteRenderer>().enabled = false;
                detectedObj.GetComponent<PolygonCollider2D>().enabled = false;
            }
            
        }

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
        if (detectedObj != null && !dManager.isSpeaking && detectedObj.GetComponent<Interactable>())
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
