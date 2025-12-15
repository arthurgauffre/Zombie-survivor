using System;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
    [Tooltip("Current round number (starts at 1)")]
    public int currentRound = 1;
    [Tooltip("If true, persist the current round across scene loads using PlayerPrefs.")]
    public bool persistRound = true;

    private const string PrefsKey = "RoundSystem_CurrentRound";

    // Event invoked when a round starts (passes the round number)
    public event Action<int> OnRoundStarted;

    void Start()
    {
        // restore persisted round if requested
        if (persistRound && PlayerPrefs.HasKey(PrefsKey))
        {
            currentRound = Mathf.Max(1, PlayerPrefs.GetInt(PrefsKey, currentRound));
        }

        // announce initial round
        OnRoundStarted?.Invoke(currentRound);
        Debug.Log($"RoundSystem: Starting at round {currentRound}");
    }

    // Call to advance to the next round
    public void NextRound()
    {
        currentRound++;
        if (persistRound)
            PlayerPrefs.SetInt(PrefsKey, currentRound);
        PlayerPrefs.Save();
        Debug.Log($"RoundSystem: Advancing to round {currentRound}");
        OnRoundStarted?.Invoke(currentRound);
    }

    // Optional: start a specific round
    public void StartRound(int round)
    {
        currentRound = Mathf.Max(1, round);
        if (persistRound)
            PlayerPrefs.SetInt(PrefsKey, currentRound);
        PlayerPrefs.Save();
        OnRoundStarted?.Invoke(currentRound);
    }

    // Clear saved progress and reset to round 1
    public void ResetProgress()
    {
        currentRound = 1;
        if (persistRound && PlayerPrefs.HasKey(PrefsKey))
        {
            PlayerPrefs.DeleteKey(PrefsKey);
            PlayerPrefs.Save();
        }
        OnRoundStarted?.Invoke(currentRound);
        Debug.Log("RoundSystem: Progress reset to round 1");
    }
}
