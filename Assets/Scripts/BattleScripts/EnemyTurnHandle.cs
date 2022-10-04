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
        this.gameObject.transform.position = new Vector3(960,540,-2);
    }

    public void AttackDone()
    {
        finishedTurn = true;
    }
}
