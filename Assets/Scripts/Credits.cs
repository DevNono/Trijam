using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    void Start() {
        StartCoroutine(GoToMainMenu());
    }

    IEnumerator GoToMainMenu() {
        yield return new WaitForSeconds(16);
        SceneManager.LoadScene("MainMenu");
    }
}
