using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniGameMove : MonoBehaviour
{

    private Vector3 startPos = Vector3.zero;

    public float speed;
    private Vector2 movePos;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetPlayer();  
    }

    public void SetPlayer()
    {
        transform.position = startPos;
        movePos = startPos;
    }

    void OnMove(InputValue value)
    {
        movePos = value.Get<Vector2>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movePos * speed * Time.fixedDeltaTime);
    }
}
