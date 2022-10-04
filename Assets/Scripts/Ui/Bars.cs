using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    public Slider bar;
    public Image fill;

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
}
