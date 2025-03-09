using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public AudioSource musicSource;
    public Image backgroundPanel;
    public float bpm = 120f;

    public AudioClip comboSound; // ✅ Add combo sound effect
    private AudioSource _audioSource;

    private float _beatInterval;
    private float _lastSpawnTime;

    private readonly float[] _lanes = new float[] { -1.5f, -0.5f, 0.5f, 1.5f };

    private ScoreManager _scoreManager;

    void Start()
    {
        _beatInterval = 60f / bpm;
        _lastSpawnTime = musicSource.time;
        _scoreManager = Object.FindFirstObjectByType<ScoreManager>();

        _audioSource = gameObject.AddComponent<AudioSource>(); // ✅ Create audio source for combo sound
    }

    void Update()
    {
        if (!musicSource.isPlaying) return;

        float currentTime = musicSource.time;

        if (currentTime - _lastSpawnTime >= _beatInterval)
        {
            SpawnTile();
            _lastSpawnTime += _beatInterval;

            StartCoroutine(BackgroundPulse());
        }
    }

    void SpawnTile()
    {
        float laneX = _lanes[Random.Range(0, _lanes.Length)];
        GameObject tile = Instantiate(tilePrefab, new Vector3(laneX, 5, 0), Quaternion.identity);

        Tile tileScript = tile.GetComponent<Tile>();
        if (tileScript != null)
        {
            tileScript.musicSource = musicSource;
        }

        // ✅ Speed Boost Feedback
        if (_scoreManager != null && _scoreManager.GetCombo() % 10 == 0 && _scoreManager.GetCombo() > 0)
        {
            StartCoroutine(SpeedBoostEffect());
            PlayComboSound();
        }
    }

    IEnumerator BackgroundPulse()
    {
        if (backgroundPanel != null)
        {
            backgroundPanel.color = new Color(0, 0, 0, 0.5f);
            yield return new WaitForSeconds(0.1f);
            backgroundPanel.color = new Color(0, 0, 0, 0f);
        }
    }

    IEnumerator SpeedBoostEffect()
    {
        if (backgroundPanel != null)
        {
            backgroundPanel.color = new Color(1f, 0f, 0f, 0.5f);
            yield return new WaitForSeconds(0.2f);
            backgroundPanel.color = new Color(0, 0, 0, 0f);
        }
    }

    // ✅ Play combo sound
    void PlayComboSound()
    {
        if (comboSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(comboSound);
        }
    }
} 
