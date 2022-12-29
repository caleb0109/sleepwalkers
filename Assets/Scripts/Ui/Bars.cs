using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    public Slider bar;
    public Image fill;

    public SpriteRenderer meter; // used for meters not in canvas 

    public void SetMax(float health)
    {
        bar.maxValue = health;
        bar.value = health;
    }

    public void ShowHealth(float health)
    {
        Debug.Log(health);
        bar.value = health;
    }

    /*public void ShowMeter(float val)
    {
        meter.transform.localScale.x = val;
    }*/
}
