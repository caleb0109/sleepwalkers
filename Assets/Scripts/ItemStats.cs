using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    private GameObject gObj;
    private float damage;
    private float heal;
    private int usage;

    public void SetStats(GameObject item)
    {
        gObj = item;
        //damage = 3;

    }
}
