using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{

    public Text notification;
    public Text title;
    public Sprite sprite;
    public Animator animator;
    public AudioSource notifFx;

    private Queue<Interactable> itemQueue;

    private void Start()
    {
        itemQueue = new Queue<Interactable>();
    }

    // sends in the notifcation to the player of picked up item
    public void NotifyUpdates(Interactable interacted)
    {
        if (animator.GetBool("IsOpen"))
        {
            itemQueue.Enqueue(interacted);
        }
        else
        {
            animator.SetBool("IsOpen", true);
            //notifFx.PlayClipAtPoint(notifFx.clip, interacted.transform.position);

            // used to keep track of how long notification has been on screen and send the notification away
            StopAllCoroutines();
            StartCoroutine(NotificationTimer());

            switch (interacted.notifType)
            {
                case Interactable.NotificationType.article:
                    notification.text = "New Article Added to Notes";
                    title.text = interacted.itemName;
                    break;

                case Interactable.NotificationType.item:
                    notification.text = "New Item Added to Inventory";
                    title.text = interacted.itemName;
                    break;

                case Interactable.NotificationType.removed:
                    notification.text = "Item removed from Inventory";
                    title.text = interacted.itemName; // temp
                    break;

                case Interactable.NotificationType.task:
                    notification.text = "To-do list Updated";
                    break;
            }
        }
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
            NotifyUpdates(itemQueue.Dequeue());
        }
    }
}
