using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int numberOfEnemies;
    public float spawnRateInterval;
    public GameObject Enemy;
}

public class enemySpawner : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    public Transform[] spawnPoints;

    private Wave currentWave;

    private void Start()
    {
        //Updates Number of Enemies in Game Manager
        for (int i = 0; i < waves.Length; i++)
        {
            gameManager.instance.updateTotalEnemyCount(waves[i].numberOfEnemies);
        }

        StartCoroutine(SpawnWave());
    }



    IEnumerator SpawnWave()
    {
        //This for loop is for the current wave
        for (int currentWaveNum = 0; currentWaveNum < waves.Length; currentWaveNum++)
        {
            currentWave = waves[currentWaveNum];
            gameManager.instance.setEnemyWaveCounter(currentWave.numberOfEnemies);
            //This for loop is to spawn in the enemies
            for (int i = 0; i < currentWave.numberOfEnemies; i++)
            {      
                Transform randomPosition = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(currentWave.Enemy, randomPosition.position, transform.rotation);
                yield return new WaitForSeconds(currentWave.spawnRateInterval);
            }
        }
    
    
    }
}