using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    private List<string> scenesInBuild;
    private string prevScene;
    private Vector3 prevPosition;

    private GameObject battleTrigger;

    // Start is called before the first frame update
    void Start()
    {
        scenesInBuild = new List<string>();

        // loading scenes curtasy of canis https://forum.unity.com/threads/getscenebybuildindex-problem.452560/
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            scenesInBuild.Add(path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1));
        }
    }

    public void NextScene()
    {
        for (int i = 0; i < scenesInBuild.Count; i++) 
        {
            if (scenesInBuild[i] == SceneManager.GetActiveScene().name && i + 1 < scenesInBuild.Count)
            {
                SceneManager.LoadSceneAsync(scenesInBuild[i + 1]);
                break;
            }
        }
    }

    // goes to battle scene and get the players prev position and the prev scene
    public void ToBattle(string currScene, Vector3 position, GameObject interacted)
    {
        prevScene = currScene;
        prevPosition = position;
        battleTrigger = interacted;
        SceneManager.LoadScene("Battle");
    }

    // after battle, move playerobj back to prev position and go back to prev scene
    public void ReturnToPrevScene(Dialogue afterBattle, bool won)
    {
        GameObject.Find("Yuichi").transform.position = prevPosition;
        SceneManager.LoadSceneAsync(prevScene);

        if (won)
        {
            Interactable iObj = battleTrigger.GetComponent<Interactable>();
            iObj.interactType = Interactable.InteractableType.Cutscene;
        }

        Debug.Log(afterBattle.diaFile);
        DialogueTrigger d = new DialogueTrigger();
        d.dialogue = afterBattle;
        d.dialogue.Start();
        d.objTrigger = battleTrigger;
        d.TriggerDialogue();
    }

    public string FindCurrentScene()
    {
        foreach(string s in scenesInBuild)
        {
            if (s == SceneManager.GetActiveScene().name)
            {
                return s;
            }
        }

        return "";
    }
}
