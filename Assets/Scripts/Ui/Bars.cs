using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Programmer: Jessica Niem
/// Date:
/// Description: 
/// </summary>
public class Bars : MonoBehaviour
{
    public Slider bar;
    public Image fill;

    public GameObject meter; // used for meters not in canvas 

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

    public void ShowMeter()
    {
        Vector3 temp = meter.transform.localScale;
        Debug.Log("before adjustment: " + temp);
        temp.x -= 1;
        Debug.Log("after adjustment: " + temp);
        meter.transform.localScale = temp;
    }
}
