using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnim : MonoBehaviour
{
    private Animator options;
    private Animator defendingAnim;
    private Animator enemyAnim;

    public Battle battle;

    private void Start()
    {

        // set the animator of the "optionBox" to active
        options = GameObject.Find("optionBox").GetComponent<Animator>();
        defendingAnim = GameObject.Find("defending").GetComponent<Animator>();
        enemyAnim = GameObject.Find("enemies").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(battle.state == BattleState.PlayerTurn)
        {
            Debug.Log("yes");
            options.SetBool("isPlayerTurn", false);
            enemyAnim.SetBool("takingDamage", true);
            defendingAnim.SetBool("isDefending", true);
        }

        //Debug.Log(battle.state);
        /*enemyAnim.SetBool("ThugTakeDamage", true);
        enemyAnim.SetBool("ThugTakeDamage", false);
        options.SetBool("isPlayerTurn", false);*/
        int enemiesDead = 0;


        /*defendingAnim.SetBool("isDefending", true);
        enemyAnim.SetBool("isAttacking", true);*/
    }

    private void FixedUpdate()
    {
        
    }

}

