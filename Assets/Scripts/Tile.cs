using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    public float speed = 3f;
    private ScoreManager _scoreManager;
    public GameObject hitEffectPrefab;
    public GameObject perfectTextPrefab; // ✅ For "PERFECT!" animation
    
    private bool _canScore = false;
    public AudioSource musicSource;

    void Start()
    {
        _scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        if (_scoreManager == null)
            Debug.LogError("ScoreManager not found in scene!");

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
            _canScore = false; // ✅ Prevent double scoring

            // ✅ Check for Perfect Hit
            float timeOffset = Mathf.Abs((musicSource.time % (60f / 120f)) - (60f / 120f));
            if (timeOffset <= 0.1f)
            {
                _scoreManager.AddScore(20);
                ShowPerfectText();
            }
            else
            {
                _scoreManager.AddScore(10);
            }

            // ✅ Play hit effect
            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);
            }

            // ✅ Animate tile shrinking
            transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject));
        }
    }

    void ShowPerfectText()
    {
        if (perfectTextPrefab != null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.LogWarning("Canvas not found in scene!");
                return;
            }

            GameObject perfectText = Instantiate(perfectTextPrefab, transform.position, Quaternion.identity, canvas.transform);
            RectTransform rectTransform = perfectText.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
                rectTransform.position = screenPosition;

                // ✅ Make sure CanvasGroup exists before using it
                CanvasGroup canvasGroup = perfectText.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    perfectText.transform.localScale = Vector3.zero;
                    perfectText.transform.DOScale(Vector3.one, 0.3f);
                    canvasGroup.DOFade(0, 0.5f).SetDelay(0.3f).OnComplete(() =>
                    {
                        Destroy(perfectText);
                    });
                }
                else
                {
                    Debug.LogWarning("CanvasGroup missing on PerfectTextPrefab");
                }
            }
        }
    }


    void Update()
    {
        transform.position += Vector3.down * (speed * Time.deltaTime);

        // ✅ Destroy off-screen tiles
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}
