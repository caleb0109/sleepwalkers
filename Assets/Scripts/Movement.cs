using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 playerInput;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + playerInput * speed * Time.fixedDeltaTime);
    }
    void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
    }

    
}
