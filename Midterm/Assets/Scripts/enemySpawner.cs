using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class enemySpawner : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    public Transform[] spawnPoints;
    private bool canSpawn = true;
    private float nextSpawnTime;
    private Wave currentWave;
    private int currentWaveNum;

    private void Start()
    {
        //Updates Number of Enemies in Game Manager
        for (int i = 0; i < waves.Length; i++)
        {
            gameManager.instance.updateTotalEnemyCount(waves[i].numberOfEnemies);
        }
    }



    private void Update()
    {
        currentWave = waves[currentWaveNum];
        SpawnWave();

        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0 && !canSpawn && currentWaveNum + 1 != waves.Length && gameManager.instance.getTotalEnemyCount() > 0 && gameManager.instance.isPaused == false)
        {
            currentWaveNum++;
            canSpawn = true;
        }
    }

    public void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            Transform randomPosition = spawnPoints[Random.Range(0, spawnPoints.Length)];
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

