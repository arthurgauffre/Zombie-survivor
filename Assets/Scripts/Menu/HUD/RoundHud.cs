using UnityEngine;
using TMPro;

public class RoundHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText;
    private RoundSystem roundSystem;
    
    void Start()
    {
        if (roundText == null)
            Debug.LogWarning("RoundHud: roundText is not assigned in the Inspector.");

        roundSystem = FindObjectOfType<RoundSystem>();
        if (roundSystem != null)
        {
            // subscribe to round changes
            roundSystem.OnRoundStarted += UpdateRound;
            // set initial value
            UpdateRound(roundSystem.currentRound);
        }
        else
        {
            Debug.LogWarning("RoundHud: No RoundSystem found in scene.");
        }
    }

    void OnDestroy()
    {
        if (roundSystem != null)
            roundSystem.OnRoundStarted -= UpdateRound;
    }

    public void UpdateRound(int currentRound)
    {
        if (roundText != null)
        {
            roundText.text = $"{currentRound}";
        }
    }
}