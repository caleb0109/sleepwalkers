using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneButtons : MonoBehaviour
{
    public GameObject nextScreen;
    public Button bttn;

    private void Start()
    {
        if (nextScreen == null)
        {
            bttn.onClick.AddListener(BackToHome);
        }
        else
        {
            bttn.onClick.AddListener(SwitchScreen);
        }
    }

    // goes to the specific screen the button is tied to
    public void SwitchScreen()
    {
        nextScreen.SetActive(true);
    }

    // sets itself inactive
    public void BackToHome()
    {
        this.transform.parent.gameObject.SetActive(false); // set the parent screen to be inactive
    }
}
