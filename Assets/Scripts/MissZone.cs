using UnityEngine;
using System.Collections;

public class MissZone : MonoBehaviour
{
    private ScoreManager _scoreManager;
    private GameManager _gameManager;

    private int _misses = 0;
    public int maxMisses = 3;
    void Start()
    {
        _scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        _gameManager = Object.FindFirstObjectByType<GameManager>(); // Find GameManager in the scene
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tile"))
        {
            if (_scoreManager != null)
            {
                _scoreManager.AddScore(-5);  // Lose points
                _scoreManager.ResetCombo();  // Reset combo
            }

            _misses++;

            // If too many misses, end the game
            if (_misses >= maxMisses)
            {
                _gameManager?.EndGame(); // Call EndGame() from GameManager
            }

            StartCoroutine(SlowMotionEffect());
            Destroy(other.gameObject);
        }
    }
    IEnumerator SlowMotionEffect()
    {
        Time.timeScale = 0.5f;  // Slow down time
        yield return new WaitForSecondsRealtime(0.2f);  // Wait for 0.2 seconds in real-time
        Time.timeScale = 1f;  // Reset time back to normal
    }
}