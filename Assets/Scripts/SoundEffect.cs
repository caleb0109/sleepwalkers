using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource notifFx;
    public AudioClip[] soundByte;
    // Start is called before the first frame update
    void Start()
    {
        notifFx = this.gameObject.GetComponent<AudioSource>();
        notifFx.clip = soundByte[0];
    }

    // Update is called once per frame
    public void PlaySound()
    {
        notifFx.Play();
        Debug.Log(notifFx.isPlaying);
    }

}