using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    private List<Scene> introScenes;
    private string prevScene;

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
        for (int i = 0; i < introScenes.Count; i++) { 
            if (introScenes[i] == SceneManager.GetActiveScene() && i + 1 < introScenes.Count)
            {
                SceneManager.LoadSceneAsync(introScenes[i + 1].ToString());
                break;
            }
        }
    }

    public void ToBattle(string currScene)
    {
        prevScene = currScene;
        SceneManager.LoadScene("Battle");
    }

    public void ReturnToPrevScene(TextAsset afterBattle)
    {
        SceneManager.LoadSceneAsync(prevScene);
    }
}
