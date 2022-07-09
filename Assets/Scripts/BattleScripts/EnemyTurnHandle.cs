using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnHandle : MonoBehaviour
{
    public bool finishedTurn;
    public int attackAmounts;
    private void Start()
    {
        finishedTurn = false;

        int attackNumb = Random.Range(0, attackAmounts);
        GetComponent<Animator>().SetInteger("AttackDex", attackNumb);
    }

    public void AttackDone()
    {
        finishedTurn = true;
    }
}
