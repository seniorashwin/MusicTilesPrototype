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

    public void AddScore(int amount)
    {
        _combo++;  // Increase combo when hitting a tile
        int totalPoints = amount * _combo;  // Multiply score by combo
        _score += totalPoints;
        UpdateScoreUI();    
        
        // ✅ Only animate if scoreText is not null and active
        if (scoreText != null && scoreText.gameObject.activeInHierarchy)
        {
            scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 5, 0.5f);
            scoreText.DOColor(Color.yellow, 0.2f).OnComplete(() =>
                scoreText?.DOColor(Color.white, 0.2f));
        }

        // ✅ Only animate if comboText is not null and active
        if (comboText != null && comboText.gameObject.activeInHierarchy)
        {
            comboText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 5, 0.5f);
        }
        
    }
    
    public void ResetCombo()
    {
        _combo = 0;  // Reset combo when missing a tile
        UpdateScoreUI();
        
        // ✅ Only shake if comboText exists and is active
        if (comboText != null && comboText.gameObject.activeInHierarchy)
        {
            comboText.transform.DOShakeScale(0.2f, 0.1f);
        }
    }
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + _score;

        if (comboText != null)
            comboText.text = "Combo: x" + _combo;  // Show combo multiplier
    }
    
    // ✅ Clean up tweens to avoid memory leaks or dangling references
    void OnDestroy()
    {
        if (scoreText != null && !ReferenceEquals(scoreText, null))  // ✅ Extra safety check
        {
            scoreText.transform.DOKill();
        }

        if (comboText != null && !ReferenceEquals(comboText, null))  // ✅ Extra safety check
        {
            comboText.transform.DOKill();
        }
    }
    
}