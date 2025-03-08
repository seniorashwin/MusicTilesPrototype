using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject retryButton;
    private bool _gameOver = false;
    
    public Transform tileParent; 
    private TileSpawner _tileSpawner;

    void Start()
    {
        retryButton.SetActive(false); // Hide retry button at start
    }

    public void EndGame()
    {
        if (!_gameOver)
        {
            _gameOver = true;
            
            // Show retry Button immediately 
            retryButton.SetActive(true);
            
            // Stop all tween and coroutines to avoid conflicts 
            StopAllCoroutines();
            DOTween.KillAll();
            
            // Stop title spawning ( if active ) 
            TileSpawner spawner = Object.FindFirstObjectByType<TileSpawner>();
            if (spawner != null)
            {
                spawner.StopSpawning(); // Add StopSpawning() in TileSpawner.cs
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

        // ✅ Start tile spawning after a small delay to avoid overlap
        Invoke(nameof(RestartSpawning), 0.5f);
    }
    
    void RestartSpawning()
    {
        if (_tileSpawner != null)
        {
            _tileSpawner.StartSpawningTiles(); // ✅ Restart tile spawning properly
        }
    }


}
