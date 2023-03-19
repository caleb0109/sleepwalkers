using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public GameObject phoneWindow;

    private Rigidbody2D rb;
    private Vector2 playerInput;
    private Animator anim;
    private Inventory inventory;

    private void Awake()
    {
        //inventory = new Inventory();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (CanMove() == false)
        {
            return;
        }
        else
        {
            anim.SetFloat("hor", playerInput.x);
            anim.SetFloat("ver", playerInput.y);

            if (playerInput.x == 1 || playerInput.x == -1 || playerInput.y == 1 || playerInput.y == -1)
            {
                anim.SetFloat("old hor", playerInput.x);
                anim.SetFloat("old ver", playerInput.y);
            }
        }
    }
    void FixedUpdate()
    {
        if (CanMove() == false)
        {
            return;
        }
        rb.MovePosition(rb.position + playerInput * speed * Time.fixedDeltaTime);
    }

    bool CanMove()
    {
        bool move = true;
        if (FindObjectOfType<Detect>().isExamining)
        {
            move = false;
        }

        /*if (FindObjectOfType<Inventory>().isOpen)
        {
            move = false;
        }*/

        if (FindObjectOfType<DialogueManager>().isSpeaking)
        {
            move = false;
        }

        if (phoneWindow.activeInHierarchy)
        {
            move = false;
        }

        return move;
    }
    void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
    }

    public void OnOpenPhone()
    {
        for (int i = 0; i < phoneWindow.transform.childCount; i++)
        {
            GameObject child = phoneWindow.transform.GetChild(i).gameObject;

            // if any of the children are active, turn them off
            if (child.activeInHierarchy && child.name != "phone")
            {
                child.SetActive(false);
            }
        }

        phoneWindow.SetActive(!phoneWindow.activeInHierarchy); // toggles phone open screen with TAB button
    }

    public void OnOpenSettings()
    {
        OnOpenPhone();

        for (int i = 0; i < phoneWindow.transform.childCount; i++)
        {
            GameObject child = phoneWindow.transform.GetChild(i).gameObject;

            if (child.name == "Settings")
            {
                child.SetActive(true);
                break;
            }
        }
    }
}
