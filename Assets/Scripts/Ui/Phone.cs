using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class Phone : MonoBehaviour
{
    public GameObject phoneWindow;
    public List<AudioClip> phoneSfx;

    private int oppositeSfx; // used to store which sound effect is next
    private AudioSource audSrc; // used to switch between sound effects

    void Start()
    {
        oppositeSfx = 0; // when at the start, On sfx is attached to audio source
        audSrc = this.GetComponent<AudioSource>();
        Debug.Log(audSrc);
    }

    public bool PhoneOpen()
    {
        return phoneWindow.activeInHierarchy;
    }

    public void TogglePhone()
    {
        SwitchAudioIndex();
        audSrc.Play();

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

    public void ToggleSettings()
    {
        TogglePhone();

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

    private void SwitchAudioIndex()
    {
        audSrc.clip = phoneSfx[oppositeSfx];

        if (oppositeSfx == 1)
        {
            oppositeSfx = 0;
        }
        else
        {
            oppositeSfx = 1;
        }
    }
}