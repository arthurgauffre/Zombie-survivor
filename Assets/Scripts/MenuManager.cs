using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    //PLayGame() and QuitGame() methods added to NewMonoBehaviourScript
    public void PlayGame()
    {
        Debug.Log("Play Game method called.");
        // Go to "Game" scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game method called.");
        // Add your game quitting logic here
        Application.Quit();
    }
}
