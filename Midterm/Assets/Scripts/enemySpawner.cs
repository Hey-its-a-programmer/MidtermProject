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
    
    
    private bool canSpawn = true;
    private float nextSpawnTime;
    private Wave currentWave;
 

    private void Start()
    {
        //Updates Number of Enemies in Game Manager
        for (int i = 0; i < gameManager.instance.waves.Length; i++)
        {
            gameManager.instance.updateTotalEnemyCount(gameManager.instance.waves[i].numberOfEnemies);
        }

    }

    


    private void Update()
    {
        if (gameManager.instance.waves.Length > 0)
        {
            currentWave = gameManager.instance.waves[gameManager.instance.currentWaveNum];
            SpawnWave();
        }


        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0 && !canSpawn && gameManager.instance.currentWaveNum + 1 != gameManager.instance.waves.Length && gameManager.instance.getTotalEnemyCount() > 0 && gameManager.instance.isPaused == false)
        {
            gameManager.instance.currentWaveNum++;
            canSpawn = true;
        }
    }

    public void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            Transform randomPosition = gameManager.instance.enemySpawnPoints[Random.Range(0, gameManager.instance.enemySpawnPoints.Length)];
            Instantiate(currentWave.Enemy, randomPosition.position, transform.rotation);

            currentWave.numberOfEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnRateInterval;

            if (currentWave.numberOfEnemies == 0)
            {
                canSpawn = false;
            }
        }

    }
}

