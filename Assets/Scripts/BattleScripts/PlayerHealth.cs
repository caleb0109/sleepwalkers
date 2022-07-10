using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int strength;

    public Bars hBar;

    private void Start()
    {
        hBar.SetMax(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            health = 0;
        }

        hBar.ShowHealth(health);
    }
}
