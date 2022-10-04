using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float maxHealth;
    public bool leftEnemy;

    private Bars hpBar;
    private Animator damageAnim;

    public GameObject[] enemiesAttacks;

    private void Start()
    {
        // set the animator
        damageAnim = this.transform.parent.gameObject.GetComponent<Animator>();

        // get a reference of the enemy hp bar
        Transform gObj = this.transform;
        for (int i = 0; i < gObj.childCount; i++)
        {
            if (gObj.GetChild(i).name == "health")
            {
                hpBar = gObj.GetChild(i).GetComponent<Bars>();
                return;
            }
        }

        hpBar.SetMax(maxHealth); // set the hpbar max
    }

    public void TakeDamage(float damage)
    {
        damageAnim.SetBool("enemyDamage", true);

        health -= damage;
        hpBar.ShowHealth(health);
        Debug.Log(hpBar);
    }
}
