using UnityEngine;
using DG.Tweening; 

public class Tile : MonoBehaviour
{
    public float speed = 3f;  // Adjust speed as needed
    private ScoreManager _scoreManager;
    public GameObject hitEffectPrefab;  // Particle effect prefab

    void Start()
    {
        _scoreManager = Object.FindFirstObjectByType<ScoreManager>(); // Get reference to ScoreManager
        if (_scoreManager == null)
            Debug.LogError("ScoreManager not found in scene!");
    }

    void OnMouseDown()
    {
        if (_scoreManager != null)
        {
            _scoreManager.AddScore(10);  // Base score is 10, but combo multiplies it
        }

        // Play hit effect manually
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();

            if (ps != null)
            {
                ps.Play();  // Play the particle effect
            }
            else
            {
                Debug.LogError("Particle System component is missing on HitEffect prefab!");
            }

            Destroy(effect, 0.5f);  // Destroy effect after it plays
        }

        // Animate shrinking before destroying the tile
        transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject));
    }


    void Update()
    {
        transform.position += Vector3.down * (3f * Time.deltaTime);  // Move tile down smoothly

        if (transform.position.y < -5f)  // Destroy tile if it falls off-screen
        {
            Destroy(gameObject);
        }
    }

}