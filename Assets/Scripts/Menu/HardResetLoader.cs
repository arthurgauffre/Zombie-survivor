using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Utility to perform a hard reset: optionally clear PlayerPrefs, destroy objects in the DontDestroyOnLoad scene
/// (unless they carry the KeepOnHardReset marker), call known reset methods on managers, then load the requested scene.
/// Usage: HardResetLoader.LoadSceneHard("map", clearPlayerPrefs:true);
/// </summary>
public class HardResetLoader : MonoBehaviour
{
    // Marker component: attach to a GameObject you want to keep across hard resets
    public class KeepOnHardReset : MonoBehaviour { }

    /// <summary>
    /// Start a hard load. This creates a temporary runner GameObject which performs the reset then loads the scene.
    /// </summary>
    public static void LoadSceneHard(string sceneName, bool clearPlayerPrefs = false)
    {
        var runner = new GameObject("HardResetLoaderRunner");
        DontDestroyOnLoad(runner);
        var loader = runner.AddComponent<HardResetLoader>();
        loader.StartCoroutine(loader.LoadCoroutine(sceneName, clearPlayerPrefs));
    }

    private IEnumerator LoadCoroutine(string sceneName, bool clearPlayerPrefs)
    {
        // restore basic runtime state
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        if (clearPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        // Destroy objects that live in the DontDestroyOnLoad scene, unless they carry the KeepOnHardReset marker
        var allGos = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in allGos)
        {
            if (go == null) continue;
            // objects moved by DontDestroyOnLoad live in a special scene named "DontDestroyOnLoad"
            if (go.scene.name == "DontDestroyOnLoad")
            {
                // Keep the runner itself
                if (go.name == "HardResetLoaderRunner") continue;
                if (go.GetComponent<KeepOnHardReset>() != null) continue;
                // Avoid destroying Unity internals by name checks if necessary
                Destroy(go);
            }
        }

        // allow one frame to flush destroys
        yield return null;

        // Attempt to reset known manager singletons via their public reset methods (if present)
        var score = FindObjectOfType<ScoreManager>();
        if (score != null)
        {
            var mi = score.GetType().GetMethod("ResetScore", BindingFlags.Public | BindingFlags.Instance);
            mi?.Invoke(score, null);
        }

        var round = FindObjectOfType<RoundSystem>();
        if (round != null)
        {
            var mi = round.GetType().GetMethod("ResetProgress", BindingFlags.Public | BindingFlags.Instance);
            mi?.Invoke(round, null);
        }

        var death = FindObjectOfType<DeathManager>();
        if (death != null)
        {
            var mi = death.GetType().GetMethod("RestoreAfterDeath", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            mi?.Invoke(death, null);
        }

        // Also call a generic IResettable.ResetState() if you implement it on managers in the scene
        foreach (var mb in FindObjectsOfType<MonoBehaviour>())
        {
            if (mb == null) continue;
            if (mb is IResettable resettable)
            {
                try { resettable.ResetState(); }
                catch { }
            }
        }

        // Finally, load the target scene (single mode clears other scenes)
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        // destroy runner after load (it will be recreated by new scene if needed)
        Destroy(gameObject);
    }
}

/// <summary>
/// Optional interface: implement on manager classes to provide a ResetState() called by HardResetLoader.
/// </summary>
public interface IResettable
{
    void ResetState();
}
