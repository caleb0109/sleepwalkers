using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurnHandle : MonoBehaviour
{
    public bool finishedTurn;
    public int attackAmounts;
    public RectTransform canvas;

    private void Start()
    {
        finishedTurn = false;

        int attackNumb = Random.Range(0, attackAmounts);
        GetComponent<Animator>().SetInteger("AttackDex", attackNumb);
        this.gameObject.transform.position = new Vector3(canvas.anchoredPosition.x,canvas.anchoredPosition.y,0);
    }

    public void AttackDone()
    {
        finishedTurn = true;
    }
}
