using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public float maxHealth;
    public bool leftEnemy;

    private Bars hpBar;
    private Animator damageAnim;
    private AudioSource audSrc;

    public GameObject[] enemiesAttacks;

    private void Start()
    {
        audSrc = this.GetComponent<AudioSource>();
        // set the animator
        damageAnim = this.transform.parent.gameObject.GetComponent<Animator>();

        // get a reference of the enemy hp bar
        Transform gObj = this.transform;
        for (int i = 0; i < gObj.childCount; i++)
        {
            if (gObj.GetChild(i).name == "health")
            {
                hpBar = gObj.GetChild(i).GetComponent<Bars>();
                hpBar.SetMax(health);
                return;
            }
        }

        hpBar.SetMax(maxHealth); // set the hpbar max
    }
    public void TakeDamage(float damage)
    {
        audSrc.Play();
        health -= damage;
        hpBar.ShowHealth(health);
        damageAnim.SetBool("enemyDamage", true);        
    }
}
