using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject firstSelectedOnPause; // assign the "Resume" button here (optional)
    [SerializeField] private MonoBehaviour[] disableOnPause; // scripts to disable while paused (e.g. camera look, movement, Shoot)
    [Tooltip("Optional: list of GameObjects to deactivate when paused. Do NOT include the PauseManager or the pause UI here.")]
    [SerializeField] private GameObject[] disableOnPauseObjects; // game objects to deactivate during pause

    [Tooltip("Optional: disable all MonoBehaviour components under these GameObjects when pausing (useful to catch camera scripts)")]
    [SerializeField] private GameObject[] autoDisableRoots;

    private bool isPaused = false;
    // track components auto-disabled so we can re-enable only those
    private List<MonoBehaviour> autoDisabledComponents = new List<MonoBehaviour>();

    void Start()
    {
        // hide the pause menu on start if assigned
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("PauseManager: Escape key pressed.");
            if (isPaused)
            {
                Debug.Log("PauseManager: Resume requested via Escape.");
                Resume();
            }
            else
            {
                Debug.Log("PauseManager: Pause requested via Escape.");
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("PauseManager: Resume() called");
        if (pauseMenuUI != null)
        {
            // disable raycast blocking so UI no longer intercepts clicks
            CanvasGroup cg = pauseMenuUI.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
            pauseMenuUI.SetActive(false);
        }

        // re-enable specified behaviours (if any)
        if (disableOnPause != null)
        {
            foreach (var comp in disableOnPause)
            {
                if (comp != null) comp.enabled = true;
            }
        }

        // reactivate any GameObjects disabled by the pause
        if (disableOnPauseObjects != null)
        {
            foreach (var go in disableOnPauseObjects)
            {
                if (go != null) go.SetActive(true);
            }
        }

        // re-enable any components we auto-disabled previously
        if (autoDisabledComponents != null && autoDisabledComponents.Count > 0)
        {
            foreach (var c in autoDisabledComponents)
            {
                if (c != null) c.enabled = true;
            }
            autoDisabledComponents.Clear();
        }

        // resume audio
        AudioListener.pause = false;

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause()
    {
        Debug.Log("PauseManager: Pause() called");
        if (pauseMenuUI != null)
        {
            // ensure a CanvasGroup exists to block raycasts
            CanvasGroup cg = pauseMenuUI.GetComponent<CanvasGroup>();
            if (cg == null) cg = pauseMenuUI.AddComponent<CanvasGroup>();

            cg.interactable = true;
            cg.blocksRaycasts = true;

            // if your PauseMenu is a Panel under an overlay Canvas, make sure it covers the whole screen (RectTransform stretch)
            // or add an Image (alpha 0) to capture clicks visually. CanvasGroup.blocksRaycasts will handle it too.

            pauseMenuUI.SetActive(true);
        }

        // disable listed behaviours to prevent camera/movement from continuing
        if (disableOnPause != null)
        {
            foreach (var comp in disableOnPause)
            {
                if (comp != null) comp.enabled = false;
            }
        }

        // deactivate specified GameObjects to fully freeze parts of the scene (do NOT include the Pause UI here)
        if (disableOnPauseObjects != null)
        {
            foreach (var go in disableOnPauseObjects)
            {
                if (go != null) go.SetActive(false);
            }
        }

        // auto-disable all MonoBehaviour components under configured roots (keeps track to re-enable later)
        if (autoDisableRoots != null)
        {
            autoDisabledComponents.Clear();
            foreach (var root in autoDisableRoots)
            {
                if (root == null) continue;
                var comps = root.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var c in comps)
                {
                    if (c == null) continue;
                    // don't disable this PauseManager if it's on the same root
                    if (c == this) continue;
                    if (c.enabled)
                    {
                        c.enabled = false;
                        autoDisabledComponents.Add(c);
                    }
                }
            }
        }

        // pause audio
        AudioListener.pause = true;

        // stop time-based updates
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // automatically select the Resume button for keyboard/joystick navigation
        if (firstSelectedOnPause != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedOnPause);
        }
    }

    // méthode à lier au OnClick() du bouton Resume dans l'Inspector
    public void OnResumeButton()
    {
        Debug.Log("PauseManager: OnResumeButton clicked");
        Resume();
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("PauseManager: ReturnToMainMenu called");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}