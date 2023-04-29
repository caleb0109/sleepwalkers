using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButtons : MonoBehaviour
{
    public string buttonName;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(SceneSwitching); // when button clicked
    }

    // switches to specific scene associated with button names
    private void SceneSwitching()
    {
        switch (buttonName)
        {
            case "New Game":
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Police Room");
                break;
            case "Load Game":
                break;
            case "Settings":
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Settings");
                break;
            case "Quit":
                Application.Quit();
                break;
            case "Return":
                if (FindObjectOfType<DontDestroy>())
                {
                    GameObject consistents = FindObjectOfType<DontDestroy>().gameObject;
                    if (consistents != null)
                    {
                        Destroy(consistents);
                    }
                }
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Start");
                break;
            case "Back":
                break;
            default:
                break;
        }
    }
}
