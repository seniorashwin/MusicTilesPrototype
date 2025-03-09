using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text comboText;

    private int _score = 0;
    private int _combo = 0;

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)  // Fixed: Use 'amount' instead of 'basePoints'
    {
        _combo++;  // Increase combo when hitting a tile
        int totalPoints = amount * _combo;  // Multiply score by combo
        _score += totalPoints;
         
        // ✅ Combo Streak Bonuses
        if (_combo % 10 == 0) // Every 10 combo hits
        {
            _score += 50; // Bonus points
            CameraShake.instance?.Shake(); // ✅ Shake camera
            Debug.Log("BONUS for combo streak!");
        }

        UpdateScoreUI();

        // ✅ Animate score text
        scoreText.transform.DOScale(1.2f, 0.2f).OnComplete(() =>
            scoreText.transform.DOScale(1f, 0.2f));
    }

    public void ResetCombo()
    {
        _combo = 0;  // Reset combo when missing a tile
        UpdateScoreUI();
    }
    
    // ✅ Add public getter for combo
    public int GetCombo()
    {
        return _combo;
    }
    
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + _score;

        if (comboText != null)
            comboText.text = "Combo: x" + _combo;  // Show combo multiplier
    }
}