using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;

    public GameObject[] enemiesAttacks;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
