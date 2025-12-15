using System;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
    [Tooltip("Current round number (starts at 1)")]
    public int currentRound = 1;

    // Event invoked when a round starts (passes the round number)
    public event Action<int> OnRoundStarted;

    void Start()
    {
        // announce initial round
        OnRoundStarted?.Invoke(currentRound);
        Debug.Log($"RoundSystem: Starting at round {currentRound}");
    }

    // Call to advance to the next round
    public void NextRound()
    {
        currentRound++;
        Debug.Log($"RoundSystem: Advancing to round {currentRound}");
        OnRoundStarted?.Invoke(currentRound);
    }

    // Optional: start a specific round
    public void StartRound(int round)
    {
        currentRound = Mathf.Max(1, round);
        OnRoundStarted?.Invoke(currentRound);
    }
}
