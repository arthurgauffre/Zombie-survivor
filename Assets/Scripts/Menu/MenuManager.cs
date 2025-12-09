using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //PLayGame() and QuitGame() methods added to NewMonoBehaviourScript
    public void PlayGame()
    {
        Debug.Log("Play Game method called.");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game method called.");
        Application.Quit();
    }
}
