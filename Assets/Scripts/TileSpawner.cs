using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public AudioSource musicSource;
    public float bpm = 120f;  // Adjust this to match your song's speed

    private float _beatInterval;
    private float _nextBeatTime = 0f;
    private bool _isSpawning = true;
    
    
    void Start()
    {
        _beatInterval = 60f / bpm;  // Time between beats
    }

    void Update()
    {
        if (!musicSource.isPlaying || !_isSpawning) return;  // Wait for music to play

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

    public void StopSpawning()
    {
        musicSource.Stop(); // Prevent further tile spawning 
    }

    public void ResetSpawning()
    {
        _nextBeatTime = Time.time + _beatInterval;
        _isSpawning = false;
    }

    
    public void StartSpawningTiles()
    {
        _isSpawning = true;  // Allow tile spawning
        _nextBeatTime = Time.time + _beatInterval;

        // Start the music if not playing
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    IEnumerator SpawnTiles()
    {
        while (true) 
        {
            SpawnTile(); 
            yield return new WaitForSeconds(0.5f); // Small delay between each spawn
        }
    }

}
