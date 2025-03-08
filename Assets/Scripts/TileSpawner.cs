using System.Collections;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public AudioSource musicSource;
    public float bpm = 120f;

    private float _beatInterval;
    private float _nextBeatTime;
    private bool _isSpawning = true;

    void Start()
    {
        _beatInterval = 60f / bpm;
    }

    void Update()
    {
        if (!musicSource.isPlaying || !_isSpawning) return;

        if (Time.time >= _nextBeatTime)
        {
            SpawnTile();
            _nextBeatTime += _beatInterval;
        }
    }

    void SpawnTile()
    {
        Instantiate(tilePrefab, new Vector3(Random.Range(-2f, 2f), 5, 0), Quaternion.identity);
    }

    public void StopSpawning()
    {
        _isSpawning = false;
        musicSource.Stop();
    }

    public void ResetSpawning()
    {
        _isSpawning = true;
        _nextBeatTime = Time.time + _beatInterval;
    }

    public void StartSpawningTiles()
    {
        _isSpawning = true;
        _nextBeatTime = Time.time + _beatInterval;

        if (!musicSource.isPlaying)
            musicSource.Play();
    }
}