using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    enum NotificationType
    {
        item,
        article,
        task,
        none
    }

    public Text notification;
    public Text title;
    public Animator animator;

    private NotificationType type;

    // Start is called before the first frame update
    void Start()
    {
        type = NotificationType.none;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NotifyUpdates(Interactable interacted)
    {
        animator.SetBool("IsOpen", true);

        // used to keep track of how long notification has been on screen and send the notification away
        StopAllCoroutines();
        StartCoroutine(NotificationTimer());

        switch(type)
        {
            case NotificationType.article:
                notification.text = "New Article Added to Notes";
                title.text = interacted.itemName;
                break;

            case NotificationType.item:
                notification.text = "New Item Added to Inventory";
                title.text = interacted.itemName;
                break;

            case NotificationType.task:
                notification.text = "To-do list Updated";
                break;
        }
    }

    // used to close the notification UI
    IEnumerator NotificationTimer()
    {

        yield return null;
    }
}
