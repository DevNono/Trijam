using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    
    [SerializeField] private GameObject tutorial;

    public void Start() {
        tutorial.SetActive(false);
    }

    public void Play()
    {
        tutorial.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            tutorial.SetActive(false);
            PlayScene();
        }
    }

    public void PlayScene() {
        SceneManager.LoadScene("GameScene");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
