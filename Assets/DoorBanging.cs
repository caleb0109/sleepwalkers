using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DoorBanging : MonoBehaviour
{
    private AudioSource audSrc;

    // Start is called before the first frame update
    void Start()
    {
        audSrc = this.GetComponent<AudioSource>();
    }

    public void StartRandomizedBanging()
    {
        audSrc.Play();
    }
}
