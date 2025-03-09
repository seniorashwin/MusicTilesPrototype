using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Tile : MonoBehaviour
{
    public float speed = 3f;
    private ScoreManager _scoreManager;
    public GameObject hitEffectPrefab;
    public GameObject perfectTextPrefab;

    private bool _canScore = false;
    public AudioSource musicSource;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider2D;

    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _scoreManager = Object.FindFirstObjectByType<ScoreManager>();

        if (_scoreManager == null)
            Debug.LogError("ScoreManager not found in scene!");

        _collider2D.enabled = false;
        Invoke(nameof(EnableCollider), 0.2f);
    }

    void EnableCollider()
    {
        _canScore = true;
        _collider2D.enabled = true;
    }

    void OnMouseDown()
    {
        if (_canScore && _scoreManager != null)
        {
            _canScore = false;

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

            // ✅ Kill tweens and shrink before destroying
            transform.DOKill();
            transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => StartCoroutine(DestroyTile()));
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

                CanvasGroup canvasGroup = perfectText.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    perfectText.transform.localScale = Vector3.zero;
                    perfectText.transform
                        .DOScale(Vector3.one * 1.2f, 0.2f)
                        .SetEase(Ease.OutBack)
                        .SetAutoKill(true)
                        .OnComplete(() =>
                        {
                            if (perfectText != null)
                            {
                                perfectText.transform.DOScale(Vector3.zero, 0.3f)
                                    .SetEase(Ease.InBack)
                                    .OnKill(() =>
                                    {
                                        if (perfectText != null)
                                        {
                                            perfectText.transform.DOKill();
                                            Destroy(perfectText);
                                        }
                                    });
                            }
                        });

                    // ✅ Fade out after bounce
                    canvasGroup.DOFade(0, 0.3f).SetDelay(0.2f).OnComplete(() =>
                    {
                        if (perfectText != null)
                        {
                            perfectText.transform.DOKill();
                            Destroy(perfectText);
                        }
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
            if (gameObject.activeInHierarchy) 
            {
                StartCoroutine(DestroyTile());
            }
        }
    }

    // ✅ Clean up routine
    IEnumerator DestroyTile()
    {
        transform.DOKill(); // ✅ Kill all active tweens
        _spriteRenderer.enabled = false; // ✅ Stop rendering immediately
        _collider2D.enabled = false; // ✅ Disable collisions

        yield return null; // ✅ Let Unity finish the frame

        Destroy(gameObject); // ✅ Clean up after frame completes
    }
}
