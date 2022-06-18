using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButtons : MonoBehaviour
{
    public string name;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(SceneSwitching); // when button clicked
    }

    // switches to specific scene associated with button names
    private void SceneSwitching()
    {
        switch (name)
        {
            case "New Game":
                SceneManager.LoadSceneAsync("Test");
                break;
            case "Load Game":
                break;
            case "Settings":
                SceneManager.LoadSceneAsync("Settings");
                break;
            case "Quit":
                Application.Quit();
                break;
            case "Return":
                SceneManager.LoadSceneAsync("Start");
                break;
            default:
                break;
        }
    }
}
