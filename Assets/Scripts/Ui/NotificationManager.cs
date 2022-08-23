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


    private Animator animator;
    private AudioSource notifFx;

    private Queue<Interactable> itemQueue;

    private void Start()
    {
        itemQueue = new Queue<Interactable>();

        animator = this.gameObject.GetComponent<Animator>();
        notifFx = this.gameObject.GetComponent<AudioSource>();
    }

    // sends in the notifcation to the player of picked up item
    public void NotifyInteractUpdate(Interactable interacted)
    {
        if (animator.GetBool("IsOpen"))
        {
            itemQueue.Enqueue(interacted);
        }
        else
        {
            animator.SetBool("IsOpen", true);

            notifFx.Play();

            // used to keep track of how long notification has been on screen and send the notification away
            StopAllCoroutines();
            StartCoroutine(NotificationTimer());

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
        notification.text = "To-Do list Updated";
        title.text = taskName;
        //iconContainer.sprite = icons[2];
    }

    // used to close the notification UI
    IEnumerator NotificationTimer()
    {
        float duration = 3f;
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

        // if there other notifications needed to show up
        if (itemQueue.Count > 0)
        {
            NotifyInteractUpdate(itemQueue.Dequeue());
        }
    }
}
