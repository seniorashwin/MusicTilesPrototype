using UnityEngine;
using DG.Tweening; 

public class Tile : MonoBehaviour
{
    public float baseSpeed = 3f;
    private float _currentSpeed;

    private ScoreManager _scoreManager;
    public GameObject hitEffectPrefab;

    public AudioSource musicSource;
    private bool _canScore = false;

    void Start()
    {
        _scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        if (_scoreManager == null)
            Debug.LogError("ScoreManager not found in scene!");

        GetComponent<Collider2D>().enabled = false;
        Invoke(nameof(EnableCollider), 0.2f);

        // ✅ Scale speed based on combo
        _currentSpeed = baseSpeed + (_scoreManager.GetCombo() * 0.1f);
        _currentSpeed = Mathf.Min(_currentSpeed, 10f); // ✅ Cap speed at 10
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
            _canScore = false;

            float timeDifference = Mathf.Abs(musicSource.time % (60f / 120f));
            if (timeDifference <= 0.1f)
            {
                _scoreManager.AddScore(20);
                Debug.Log("PERFECT HIT!");
            }
            else
            {
                _scoreManager.AddScore(10);
            }

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

            transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject));
        }
    }

    void Update()
    {
        transform.position += Vector3.down * (_currentSpeed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
} 
