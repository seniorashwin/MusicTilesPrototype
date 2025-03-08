using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public AudioSource musicSource;
    public Image backgroundPanel;  // UI Panel to pulse
    public float bpm = 120f;

    private float _beatInterval;
    private float _nextBeatTime = 0f;

    void Start()
    {
        _beatInterval = 60f / bpm;
    }

    void Update()
    {
        if (!musicSource.isPlaying) return;

        if (Time.time >= _nextBeatTime)
        {
            SpawnTile();
            _nextBeatTime += _beatInterval;

            StartCoroutine(BackgroundPulse()); // Trigger pulse
        }
    }

    void SpawnTile()
    {
        Instantiate(tilePrefab, new Vector3(Random.Range(-2f, 2f), 5, 0), Quaternion.identity);
    }

    IEnumerator BackgroundPulse()
    {
        if (backgroundPanel != null)
        {
            backgroundPanel.color = new Color(0, 0, 0, 0.5f);  // Darken screen slightly
            yield return new WaitForSeconds(0.1f);
            backgroundPanel.color = new Color(0, 0, 0, 0f);  // Reset
        }
    }
}