using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 playerInput;
    private Animator anim;
    private Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory();
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
        if (FindObjectOfType<Inventory>().isOpen)
        {
            move = false;
        }
        return move;
    }
    void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
    }
 
}
