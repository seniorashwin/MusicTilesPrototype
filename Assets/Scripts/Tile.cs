using UnityEngine;
using DG.Tweening; 

public class Tile : MonoBehaviour
{
    public float speed = 3f;
    private ScoreManager _scoreManager;
    public GameObject hitEffectPrefab;
    
    private bool _canScore = false;

    void Start()
    {
        _scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        if (_scoreManager == null)
            Debug.LogError("ScoreManager not found in scene!");

        // ✅ Delay collider activation to avoid instant scoring
        GetComponent<Collider2D>().enabled = false;
        Invoke(nameof(EnableCollider), 0.2f);
    }

    void EnableCollider()
    {
        _canScore = true;
        GetComponent<Collider2D>().enabled = true;
    }

    void OnMouseDown()
    {
        if (_canScore && _scoreManager != null)
        {
            _canScore = false;  // ✅ Prevent double scoring
            _scoreManager.AddScore(10);

            // ✅ Play hit effect manually
            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play(); 
                }

                Destroy(effect, 0.5f); 
            }

            // ✅ Animate shrinking before destroying the tile
            transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject));
        }
    }

    void Update()
    {
        transform.position += Vector3.down * (speed * Time.deltaTime);

        // ✅ Destroy off-screen tiles properly
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}