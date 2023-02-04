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

    // used for gameobjects meters
    public void ShowMeter()
    {
        // gets the localScale, adjusts the x and then sets the localScale to the new adjustment
        Vector3 temp = meter.transform.localScale;
        temp.x += 0.1f;
        meter.transform.localScale = temp;
    }
}
