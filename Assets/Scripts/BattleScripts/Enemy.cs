using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float maxHealth;

    public GameObject[] enemiesAttacks;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
