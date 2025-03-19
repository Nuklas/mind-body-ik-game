using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public GameObject enemyPrefab;
    public int enemyCount;
    public float spawnDelay = 0.5f;
    public Transform[] spawnPoints;
}

public class EnemyWaveSpawner : MonoBehaviour
{
    public EnemyWave[] waves;
    public KeyCode spawnKey = KeyCode.Space;
    
    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    
    void Update()
    {
        // Check for key press to spawn next wave
        if (Input.GetKeyDown(spawnKey) && !isSpawning)
        {
            StartCoroutine(SpawnWave(currentWaveIndex));
            
            // Move to next wave
            currentWaveIndex = (currentWaveIndex + 1) % waves.Length;
        }
    }
    
    IEnumerator SpawnWave(int waveIndex)
    {
        if (waveIndex >= waves.Length)
        {
            Debug.LogWarning("Wave index out of range!");
            yield break;
        }
        
        isSpawning = true;
        
        EnemyWave wave = waves[waveIndex];
        Debug.Log("Spawning wave: " + wave.waveName);
        
        for (int i = 0; i < wave.enemyCount; i++)
        {
            // Randomly select a spawn point from the available options
            Transform spawnPoint = wave.spawnPoints[Random.Range(0, wave.spawnPoints.Length)];
            
            // Spawn the enemy
            GameObject enemy = Instantiate(wave.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Make sure enemy is on the Enemy layer
            enemy.layer = LayerMask.NameToLayer("Enemy");
            
            // Wait for the specified delay before spawning the next enemy
            yield return new WaitForSeconds(wave.spawnDelay);
        }
        
        isSpawning = false;
    }
}