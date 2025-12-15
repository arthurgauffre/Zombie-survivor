using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject deathMenuUI;
    [SerializeField] private GameObject firstSelectedOnDeath;

    [Header("Disable On Death")]
    [SerializeField] private MonoBehaviour[] disableOnDeath; // scripts to disable (player, look, shoot...)
    [SerializeField] private GameObject[] disableOnDeathObjects; // gameobjects to deactivate (spawners, etc.)
    [SerializeField] private GameObject[] autoDisableRoots; // optional roots: disable all MonoBehaviours under these

    private List<MonoBehaviour> autoDisabledComponents = new List<MonoBehaviour>();
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (deathMenuUI != null)
            deathMenuUI.SetActive(false);
    }

    // Call this from your player-death logic
    public void OnPlayerDeath()
    {
        if (isGameOver) return;
        isGameOver = true;
        ShowDeathScreen();
    }

    private void ShowDeathScreen()
    {
        // show UI and block raycasts
        if (deathMenuUI != null)
        {
            CanvasGroup cg = deathMenuUI.GetComponent<CanvasGroup>();
            if (cg == null) cg = deathMenuUI.AddComponent<CanvasGroup>();
            cg.interactable = true;
            cg.blocksRaycasts = true;
            deathMenuUI.SetActive(true);
        }

        // disable listed components
        if (disableOnDeath != null)
        {
            foreach (var comp in disableOnDeath)
            {
                if (comp != null) comp.enabled = false;
            }
        }

        // deactivate listed gameobjects
        if (disableOnDeathObjects != null)
        {
            foreach (var go in disableOnDeathObjects)
            {
                if (go != null) go.SetActive(false);
            }
        }

        // auto-disable MonoBehaviours under configured roots
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
                    if (c == this) continue;
                    if (c.enabled)
                    {
                        c.enabled = false;
                        autoDisabledComponents.Add(c);
                    }
                }
            }
        }

        // pause audio and time
        AudioListener.pause = true;
        Time.timeScale = 0f;

        // show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // select default button for controller/keyboard
        if (firstSelectedOnDeath != null && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedOnDeath);
        }
    }

    // Restart current level
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        if (deathMenuUI != null)
            deathMenuUI.SetActive(false);
        // optionally re-enable previously disabled components/objects
        RestoreAfterDeath();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Return to main menu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        RestoreAfterDeath();
        SceneManager.LoadScene("MainMenu");
    }

    // Quit game (builds)
    public void QuitGame()
    {
        Application.Quit();
    }

    private void RestoreAfterDeath()
    {
        // re-enable components disabled by auto-disable
        if (autoDisabledComponents != null && autoDisabledComponents.Count > 0)
        {
            foreach (var c in autoDisabledComponents)
            {
                if (c != null) c.enabled = true;
            }
            autoDisabledComponents.Clear();
        }

        // re-enable listed components
        if (disableOnDeath != null)
        {
            foreach (var comp in disableOnDeath)
            {
                if (comp != null) comp.enabled = true;
            }
        }

        // reactivate listed objects
        if (disableOnDeathObjects != null)
        {
            foreach (var go in disableOnDeathObjects)
            {
                if (go != null) go.SetActive(true);
            }
        }

        // resume audio
        AudioListener.pause = false;
        isGameOver = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
