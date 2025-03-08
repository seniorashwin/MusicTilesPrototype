using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text comboText; 
    
    private int score = 0;
    private int combo = 0;

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        combo++;  // Increase combo when hitting a tile
        int totalPoints = amount * combo;  // Multiply score by combo
        score += totalPoints;
        UpdateScoreUI();    
        
        
        // Animate text when score changes
        scoreText.transform.DOScale(1.2f, 0.2f).OnComplete(() => 
            scoreText.transform.DOScale(1f, 0.2f));
    }
    
    public void ResetCombo()
    {
        combo = 0;  // Reset combo when missing a tile
        UpdateScoreUI();
    }
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (comboText != null)
            comboText.text = "Combo: x" + combo;  // Show combo multiplier
    }
}