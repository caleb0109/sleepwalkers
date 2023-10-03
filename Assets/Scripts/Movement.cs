using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public List<AudioClip> footsteps;

    private Rigidbody2D rb;
    private Vector2 playerInput;
    private Animator anim;
    private AudioSource audSrc;
    private Inventory inventory;
    private Phone phoneWindow;

    private bool inCutscene;

    private void Awake()
    {
        //inventory = new Inventory();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audSrc = GetComponent<AudioSource>();

        audSrc.clip = footsteps[0];

        inCutscene = false;

        phoneWindow = FindObjectOfType<Phone>();
    }

    void Update()
    {
        if (!CanMove())
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

            if (!audSrc.isPlaying && (playerInput.x != 0 || playerInput.y != 0))
            {
                audSrc.Play();
            }
        }
    }
    void FixedUpdate()
    {
        if (!CanMove())
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

        if (FindObjectOfType<DialogueManager>().isSpeaking)
        {
            move = false;
        }

        if (phoneWindow.PhoneOpen())
        {
            move = false;
        }

        if (inCutscene)
        {
            move = false;
        }

        // sets the animator to stop moving.
        if (!move)
        {
            anim.SetFloat("hor", 0);
            anim.SetFloat("ver", 0);
        }

        return move;
    }

    // toggles inCutscene to prevent player from moving.
    public void ToggleInCutscene()
    {
        inCutscene = !inCutscene;
    }

    void OnMove(InputValue value)
    {

            playerInput = value.Get<Vector2>();
        
    }

    void OnOpenPhone()
    {
        phoneWindow.TogglePhone();
    }

    void OnOpenSettings()
    {
        phoneWindow.ToggleSettings();
    }
}
