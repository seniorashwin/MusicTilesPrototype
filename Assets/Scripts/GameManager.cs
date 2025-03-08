using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject retryButton;
    public Transform tileParent;

    private TileSpawner _tileSpawner;
    private bool _gameOver = false;

    void Start()
    {
        retryButton.SetActive(false); // Hide retry button at start
        _tileSpawner = Object.FindFirstObjectByType<TileSpawner>();
    }

    public void EndGame()
    {
        if (!_gameOver)
        {
            _gameOver = true;
            retryButton.SetActive(true);

            // Stop all tween and coroutines
            StopAllCoroutines();
            DOTween.KillAll();

            if (_tileSpawner != null)
            {
                _tileSpawner.StopSpawning(); 
            }
        }
    }

    public void Retry()
    {
        // ✅ Clear existing tiles before retrying
        foreach (Transform child in tileParent)
        {
            Destroy(child.gameObject);
        }

        // ✅ Reset timeScale to normal if needed
        Time.timeScale = 1f;

        // ✅ Restart tile spawning after a short delay
        Invoke(nameof(RestartSpawning), 0.5f);

        // ✅ Reset game state
        _gameOver = false;
    }

    void RestartSpawning()
    {
        if (_tileSpawner != null)
        {
            _tileSpawner.ResetSpawning(); // Properly reset spawning
            _tileSpawner.StartSpawningTiles(); // Start spawning tiles again
        }
    }
}