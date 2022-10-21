using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class NotificationManager : MonoBehaviour
{

    public Text notification;
    public Text title;
    public Image iconContainer;
    public Sprite[] icons;

    [HideInInspector] public Animator animator;
    private AudioSource notifFx;
    public AudioClip[] soundByte; // used to switch between audio clips

    private Queue<Interactable> itemQueue;

    private string nextTask; // used if the animator's open and needs to queue task update

    private Tutorial tutorManager;

    private void Start()
    {
        nextTask = "";

        itemQueue = new Queue<Interactable>();

        animator = this.gameObject.GetComponent<Animator>();
        notifFx = this.gameObject.GetComponent<AudioSource>();

        tutorManager = FindObjectOfType<Tutorial>();
    }

    // sends in the notifcation to the player of picked up item
    public void NotifyInteractUpdate(Interactable interacted)
    {
        notifFx.clip = soundByte[0]; // regular notification
        if (CheckAnimatorOpen())
        {
            itemQueue.Enqueue(interacted);
        }
        else
        {
            StartNotifAnim();
            switch (interacted.notifType)
            {
                case Interactable.NotificationType.article:
                    notification.text = "New Article Added to Notes";
                    title.text = interacted.itemName;
                    iconContainer.sprite = icons[0]; // notes icon
                    break;

                case Interactable.NotificationType.item:
                    notification.text = "New Item Added to Inventory";
                    title.text = interacted.itemName;
                    iconContainer.sprite = icons[1]; // inventory icon
                    break;

                case Interactable.NotificationType.removed:
                    notification.text = "Item removed from Inventory";
                    title.text = interacted.itemName; // temp
                    iconContainer.sprite = icons[1];
                    break;
            }
        }
    }

    // update player about tasks
    public void NotifyTaskUpdate(string taskName)
    {
        if (CheckAnimatorOpen())
        {
            nextTask = taskName;
        }
        else
        {
            StartNotifAnim();
            notification.text = "To-Do list Updated";
            title.text = taskName;
            //iconContainer.sprite = icons[2];
        }
    }

    public void ShowTutorial(string tutorialName, string howTo)
    {
        notifFx.clip = soundByte[1]; // tutorial notification

        StartNotifAnim();
        //StopAllCoroutines();
        //StartCoroutine(tutorManager.TutorialDuration(tutorialName));
        notification.text = tutorialName;
        title.text = howTo;
        iconContainer.sprite = icons[0]; // notes icon
    }

    private void StartNotifAnim()
    {
        animator.SetBool("IsOpen", true);

        notifFx.Play();

        // used to keep track of how long notification has been on screen and send the notification away
        StopAllCoroutines();
        StartCoroutine(NotificationTimer());
    }

    private bool CheckAnimatorOpen()
    {
        return animator.GetBool("IsOpen");
    }

    // used to close the notification UI
    public IEnumerator NotificationTimer()
    {
        float duration = 5f;
        while(duration > 0f)
        {
            duration -= Time.deltaTime;
            animator.SetFloat("Timer", duration);

            if (animator.GetFloat("Timer") < 1f)
            {
                animator.SetBool("IsOpen", false);
            }

            yield return null;
        }

        if (nextTask != "")
        {
            NotifyTaskUpdate(nextTask);
            nextTask = "";
        }
        // if there other notifications needed to show up
        else if (itemQueue.Count > 0)
        {
            NotifyInteractUpdate(itemQueue.Dequeue());
        }
        else if (tutorManager && tutorManager.tutorialNames.Count > 0)
        {
            tutorManager.CheckTutorialCondition(name);
        }
        else if (tutorManager.tutorialNames.Count == 0)
        {
            tutorManager.PhoneTutorial();
        }
    }
}
