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

    void Start()
    {
        // get a reference of the enemy hp bar
        Transform gObj = this.gameObject.transform;
        for (int i = 0; i < gObj.childCount; i++)
        {
            if (gObj.GetChild(i).name == "health")
            {
                hpBar = gObj.GetChild(i).GetComponent<Bars>();
                return;
            }
        }

        hpBar.SetMax(maxHealth); // set the hpbar max

        // set the animator
        damageAnim = this.gameObject.GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(damageAnim);

        //damageAnim.SetBool("takingDamage", true);

        health -= damage;
        hpBar.ShowHealth(health);
        Debug.Log(hpBar);

        damageAnim.SetBool("isLefEnemy", leftEnemy);
    }
}
