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
    public Wave[] waves;
    public Transform[] enemySpawnPoints;
    public int currentWaveNum;

    private bool canSpawn = true;
    private bool canSpawnWave = true;
    private float nextSpawnTime;
    private Wave currentWave;


    private void Start()
    {
        //Updates Number of Enemies in Game Manager
        if (waves.Length > 0)
        {
            for (int i = 0; i < waves.Length; i++)
            {
                gameManager.instance.updateTotalEnemyCount(waves[i].numberOfEnemies);
            }

        }

    }

    


    private void Update()
    {
        if (waves.Length == 0)
        {
            canSpawnWave = false;
        }

        else if (waves.Length > 0 && canSpawnWave)
        {
            currentWave = waves[currentWaveNum];
            SpawnWave();
        }



        if (gameManager.instance.EnemiesInWaveCount == 0 && !canSpawn && currentWaveNum + 1 != waves.Length && gameManager.instance.TotalEnemyCount > 0 && !gameManager.instance.isPaused && canSpawnWave)
        {
            currentWaveNum++;
            canSpawn = true;
        }
    }

    public void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            Transform randomPosition = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
            Instantiate(currentWave.Enemy, randomPosition.position, transform.rotation);

            currentWave.numberOfEnemies--;
            gameManager.instance.EnemiesInWaveCount++;
            nextSpawnTime = Time.time + currentWave.spawnRateInterval;

            if (currentWave.numberOfEnemies == 0)
            {
                canSpawn = false;
            }
        }

    }
}

