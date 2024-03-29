
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
    public Canvas canvas;

    public TextAsset[] diaFiles;

    public GameObject walls;
    public MiniGameMove player;
    public PlayerHealth playerH;

    private List<Dialogue> dia;
    private bool enemyStatusCheck;

    private Animator options;
    private Animator defendingAnim;
    private Animator enemyAnim;

    private void Awake()
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

        options = GameObject.Find("optionBox").GetComponent<Animator>();
        defendingAnim = GameObject.Find("defending").GetComponent<Animator>();
        enemyAnim = GameObject.Find("enemies").GetComponent<Animator>();

        enemyAnim.SetInteger("numEnemies", enemiesInBattle.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BattleState.Start)
        {
            state = BattleState.PlayerTurn;
            enemyAnim.SetBool("enemyLeave", false);
            enemyAnim.SetBool("enemyReturn", true);
            enemyAnim.SetBool("enemyDamage", false);
        }
        else if (state == BattleState.PlayerTurn)
        {
            if (!enemyStatusCheck)
            {
                
                int index = SearchForDiaFile("Battle");
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
                    DialogueTrigger d = this.gameObject.GetComponent<DialogueTrigger>();

                    foreach (Enemy e in enemiesInBattle)
                    {
                        float f = e.health / e.maxHealth;

                        if (f <= .08f)
                        {
                            d.dialogue.sentences = new List<string>() { dia[index].CharaLines[4] };
                        }
                        else if (f <= .25f)
                        {
                            d.dialogue.sentences = new List<string>() { dia[index].CharaLines[3] };
                        }
                        else if (f <= .5f)
                        {
                            d.dialogue.sentences = new List<string>() { dia[index].CharaLines[2] };
                        }
                        else if (f <= .75f)
                        {
                            d.dialogue.sentences = new List<string>() { dia[index].CharaLines[1] };
                        }
                        else if (f <= 1.0f)
                        {
                            d.dialogue.sentences = new List<string>() { dia[index].CharaLines[0] };
                        }
                    }

                    d.TriggerSentence();
                }

                enemyStatusCheck = true;
            }
        }
        else if (state == BattleState.EnemyTurn)
        {
            if (enemiesInBattle.Length <= 0)
            {
                EnemyFinishedTurn();
            }
            else
            {
                if (!enemyActed)
                {
                    defendingAnim.SetBool("isDefending", true);
                    enemyAnim.SetBool("enemyLeave", true);

                    player.gameObject.SetActive(true);
                    walls.SetActive(true);
                    player.SetPlayer();
                    foreach (Enemy enem in enemiesInBattle)
                    {
                        int atkNumb = Random.Range(0, enem.enemiesAttacks.Length);
                        GameObject attackk = Instantiate(enem.enemiesAttacks[atkNumb], Vector3.zero, Quaternion.identity);
                        attackk.transform.SetParent(canvas.transform);
                        //attackk.GetComponentInChildren<RectTransform>().localScale = canvas.GetComponentInChildren<RectTransform>().localScale;
                    }
                    enemyAttacks = GameObject.FindGameObjectsWithTag("Enemy");
                    enemyActed = true;
                }
                else
                {

                    bool enemyFin = true;
                    foreach (GameObject enem in enemyAttacks)
                    {
                        if (!enem.GetComponent<EnemyTurnHandle>().finishedTurn)
                        {
                            enemyFin = false;
                        }
                    }
                    if (enemyFin)
                    {
                        player.gameObject.SetActive(false);
                        EnemyFinishedTurn();
                    }
                }
            }
        }
        else if (state == BattleState.FinishedTurn)
        {
            
            defendingAnim.SetBool("isDefending", false);
            options.SetBool("isPlayerTurn", true);

            player.gameObject.SetActive(false);
            walls.SetActive(false);
            state = BattleState.Start;
        }
        else if (state == BattleState.Win)
        {
           FindObjectOfType<Scenes>().ReturnToPrevScene(dia[SearchForDiaFile("Success")], true);
        }
        else if (state == BattleState.Lose)
        {
            FindObjectOfType<Scenes>().ReturnToPrevScene(dia[SearchForDiaFile("Defeat")], false);
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
        enemyAnim.SetBool("enemyReturn", false);
        
        options.SetBool("isPlayerTurn", false);
        int enemiesDead = 0;
        for (int i = 0; i < enemiesInBattle.Length; i++)
        {
            enemiesInBattle[i].TakeDamage(playerH.strength);
            if (enemiesInBattle[i].health <= 0)
            {
                enemiesInBattle[i].gameObject.SetActive(false);
                enemiesDead += 1;
            }
        }

        

        PlayerFinishTurn(enemiesDead);
    }

    public void PlayerFinishTurn(int dead)
    {
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
        
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            Destroy(enemyAttacks[i]);
        }
        enemyActed = false;
        state = BattleState.FinishedTurn;

        enemyStatusCheck = false;
    }

    // temporary
    public void OpenItems()
    {
        DialogueTrigger d = gameObject.GetComponent<DialogueTrigger>();
        d.dialogue.sentences = new List<string>() { "I don't have any items to use." };
        d.TriggerSentence();
    }

    public void Run()
    {
        DialogueTrigger d = gameObject.GetComponent<DialogueTrigger>();
        d.dialogue.sentences = new List<string>() { "There's no where to run." };
        d.TriggerSentence();
    }

    public int SearchForDiaFile(string containedWord)
    {
        int index = 0;

        for (int i = 0; i < dia.Count; i++)
        {
            if (dia[i].diaFile.name.Contains(containedWord))
            {
                Debug.Log("I found the file. It's: " + dia[i].diaFile.name);
                return i;
            }
        }

        return index;
    }

}
