using UnityEngine;

public class Retry : MonoBehaviour
{
    public void RetryGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("map");
    }
}
