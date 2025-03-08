using UnityEngine;
using System.Collections.Generic;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public AudioSource musicSource;
    public float bpm = 120f;  // Adjust this to match your song's speed

    private float _beatInterval;
    private float _nextBeatTime = 0f;

    void Start()
    {
        _beatInterval = 60f / bpm;  // Time between beats
    }

    void Update()
    {
        if (!musicSource.isPlaying) return;  // Wait for music to play

        if (Time.time >= _nextBeatTime)
        {
            SpawnTile();
            _nextBeatTime += _beatInterval;  // Schedule next beat
        }
    }

    void SpawnTile()
    {
        Instantiate(tilePrefab, new Vector3(Random.Range(-2f, 2f), 5, 0), Quaternion.identity);
    }
    
}
