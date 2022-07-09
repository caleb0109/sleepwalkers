using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackDmg : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerHealth>())
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
