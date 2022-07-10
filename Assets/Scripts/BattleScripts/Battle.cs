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

    public TextAsset[] diaFiles;

    public GameObject playerUi;
    public GameObject engageUI;
    public GameObject walls;
    public MiniGameMove player;
    public PlayerHealth playerH;

    private List<Dialogue> dia;
    private bool enemyStatusCheck;


    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.Start;
        enemyActed = false;
        enemyStatusCheck = false;

        dia = new List<Dialogue>();

        for (int i = 0; i < diaFiles.Length; i++)
        {
            Dialogue d = new Dialogue(); // create new Dialogue
            d.diaFile = diaFiles[i]; // add a dialogue file
            dia.Add(d); // add it the list
            dia[i].Start(); // load the files
        }
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
            if (!enemyStatusCheck)
            {
                int index = 0;
                bool fileFound = false;

                for (int i = 0; i < dia.Count; i++)
                {
                    if (dia[i].diaFile.name.Contains("Battle"))
                    {
                        index = i;
                        fileFound = true;
                        break;
                    }
                }

                if (fileFound)
                {
                    DialogueTrigger d = gameObject.GetComponent<DialogueTrigger>();

                    foreach (Enemy e in enemiesInBattle)
                    {
                        float f = e.health / e.maxHealth;

                        if (f <= .08f)
                        {
                            d.dialogue.sentences = new string[] { dia[index].CharaLines[4] };
                        }
                        else if (f <= .25f)
                        {
                            d.dialogue.sentences = new string[] { dia[index].CharaLines[3] };
                        }
                        else if (f <= .5f)
                        {
                            d.dialogue.sentences = new string[] { dia[index].CharaLines[2] };
                        }
                        else if (f <= .75f)
                        {
                            d.dialogue.sentences = new string[] { dia[index].CharaLines[1] };
                        }
                        else if (f <= 1.0f)
                        {
                            d.dialogue.sentences = new string[] { dia[index].CharaLines[0] };
                        }
                    }

                    d.TriggerSentence();
                }

                enemyStatusCheck = true;
            }             
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
        engageUI.SetActive(true);
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
        engageUI.SetActive(false);

        enemyStatusCheck = false;
    }

    // temporary
    public void OpenItems()
    {
        DialogueTrigger d = gameObject.GetComponent<DialogueTrigger>();
        d.dialogue.sentences = new string[] { "I don't have any items to use." };
        d.TriggerSentence();
    }

    public void Run()
    {
        DialogueTrigger d = gameObject.GetComponent<DialogueTrigger>();
        d.dialogue.sentences = new string[] { "There's no where to run." };
        d.TriggerSentence();
    }

}
