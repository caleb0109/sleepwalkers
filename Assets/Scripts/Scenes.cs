using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    private List<Scene> introScenes;
    private string prevScene;
    private Vector3 prevPosition;

    // Start is called before the first frame update
    void Start()
    {
        introScenes = new List<Scene>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            introScenes.Add(SceneManager.GetSceneAt(i));
        }
    }

    public void NextScene()
    {
        Debug.Log("Next scene we go!");

        for (int i = 0; i < introScenes.Count; i++) { 
            if (introScenes[i] == SceneManager.GetActiveScene() && i + 1 < introScenes.Count)
            {
                SceneManager.LoadSceneAsync(introScenes[i + 1].ToString());
                break;
            }
        }
    }

    // goes to battle scene and get the players prev position and the prev scene
    public void ToBattle(string currScene, Vector3 position)
    {
        prevScene = currScene;
        prevPosition = position;
        SceneManager.LoadScene("Battle");
    }

    // after battle, move playerobj back to prev position and go back to prev scene
    public void ReturnToPrevScene(Dialogue afterBattle)
    {
        GameObject.Find("Yuichi").transform.position = prevPosition;
        SceneManager.LoadSceneAsync(prevScene);

        DialogueTrigger d = new DialogueTrigger();
        d.dialogue = afterBattle;
        //d.TriggerDialogue();
    }
}
