using UnityEngine;

public class Retry : MonoBehaviour
{
    public void RetryGame()
    {
        // Perform a hard reset and load the map scene. This clears persistent managers and PlayerPrefs.
        HardResetLoader.LoadSceneHard("map", true);
    }
}
