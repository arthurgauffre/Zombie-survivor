using UnityEngine;

public class ScoreHud : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = ScoreManager.Instance;
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged += UpdateScore;
            // initialize display
            UpdateScore(scoreManager.CurrentScore);
        }
        else
        {
            Debug.LogWarning("ScoreHud: No ScoreManager found in scene. Create one or attach it to a GameObject.");
        }
    }

    void OnDestroy()
    {
        if (scoreManager != null)
            scoreManager.OnScoreChanged -= UpdateScore;
    }

    // called when score changes
    void UpdateScore(int currentScore)
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}
