using UnityEngine;
using System.Collections;

public class MissZone : MonoBehaviour
{
    private ScoreManager _scoreManager;

    void Start()
    {
        _scoreManager = Object.FindFirstObjectByType<ScoreManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tile"))
        {
            if (_scoreManager != null)
            {
                _scoreManager.AddScore(-5);  // Lose points for missing
                _scoreManager.ResetCombo();  // Reset combo
            }

            StartCoroutine(SlowMotionEffect());  // Trigger slow-motion

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