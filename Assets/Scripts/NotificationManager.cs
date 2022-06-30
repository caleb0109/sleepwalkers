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

    // sends in the notifcation to the player of picked up item
    public void NotifyUpdates(Interactable interacted)
    {
        animator.SetBool("IsOpen", true);

        // used to keep track of how long notification has been on screen and send the notification away
        StopAllCoroutines();
        StartCoroutine(NotificationTimer());

        switch(interacted.notifType)
        {
            case Interactable.NotificationType.article:
                notification.text = "New Article Added to Notes";
                title.text = interacted.itemName;
                break;

            case Interactable.NotificationType.item:
                notification.text = "New Item Added to Inventory";
                title.text = interacted.itemName;
                break;

            case Interactable.NotificationType.task:
                notification.text = "To-do list Updated";
                break;
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
    }
}
