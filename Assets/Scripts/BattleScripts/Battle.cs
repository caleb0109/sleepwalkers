using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    FinishedTurn,
    Win,
    Lose
}
public class Battle : MonoBehaviour
{
    public BattleState state;

    public Enemy[] enemiesInBattle;
    private bool enemyActed;
    private GameObject[] enemyAttacks;

    public GameObject playerUi;
    public GameObject walls;
    public MiniGameMove player;
    public PlayerHealth playerH;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.Start;
        enemyActed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BattleState.Start)
        {
            playerUi.SetActive(true);
            state = BattleState.PlayerTurn;
        }
        else if (state == BattleState.PlayerTurn)
        {

        }
        else if(state == BattleState.EnemyTurn)
        {
            if(enemiesInBattle.Length <= 0)
            {
                EnemyFinishedTurn();
            }
            else
            {
                if (!enemyActed)
                {

                    player.gameObject.SetActive(true);
                    walls.SetActive(true);
                    player.SetPlayer();
                    int repeat = 0;
                    foreach (Enemy enem in enemiesInBattle)
                    {
                        int atkNumb = Random.Range(0, enem.enemiesAttacks.Length);
                        repeat = atkNumb;
                        Instantiate(enem.enemiesAttacks[atkNumb], Vector3.zero, Quaternion.identity);
                    }
                    enemyAttacks = GameObject.FindGameObjectsWithTag("Enemy");
                    enemyActed = true;
                }
                else
                {
                    bool enemyFin = true;
                    foreach(GameObject enem in enemyAttacks)
                    {
                        if(!enem.GetComponent<EnemyTurnHandle>().finishedTurn)
                        {
                            enemyFin = false;
                        }
                    }
                    if(enemyFin)
                    {
                        EnemyFinishedTurn();
                    }
                }
            }
        }
        else if(state == BattleState.FinishedTurn)
        {
            player.gameObject.SetActive(false);
            walls.SetActive(false);
            state = BattleState.Start;
        }
        else if(state == BattleState.Win)
        {

        }
        else if(state == BattleState.Lose)
        {

        }
    }

    private void FixedUpdate()
    {
        if (playerH.health <= 0)
        {
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                Destroy(enemyAttacks[i]);
            }
            player.gameObject.SetActive(false);
            walls.SetActive(false);
            state = BattleState.Lose;
        }
    }
    public void PlayerAct()
    {
        int enemiesDead = 0;
        for(int i = 0; i < enemiesInBattle.Length; i++)
        {
            enemiesInBattle[i].TakeDamage(playerH.strength);
            Debug.Log(enemiesInBattle[i].health);
            if(enemiesInBattle[i].health <= 0)
            {
                enemiesInBattle[i].gameObject.SetActive(false);
                enemiesDead += 1;
            }
        }
        

        PlayerFinishTurn(enemiesDead);
    }
    public void PlayerFinishTurn(int dead)
    {
        playerUi.SetActive(false);
        if (dead == enemiesInBattle.Length)
        {
            state = BattleState.Win;
        }
        else
        {
            state = BattleState.EnemyTurn;
        }
        
    }
    public void EnemyFinishedTurn()
    {
        for(int i = 0; i < enemyAttacks.Length; i++)
        {
            Destroy(enemyAttacks[i]);
        }
        enemyActed = false;
        state = BattleState.FinishedTurn;
    }
}
