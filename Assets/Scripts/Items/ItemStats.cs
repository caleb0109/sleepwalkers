using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    protected GameObject gObj;
    protected float damage;
    protected float heal;
    protected int usage;

    public void SetStats(GameObject item, int usage)
    {
        gObj = item;
    }


}
