using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Wave
{
    public string waveName;
    public int numberOfEnemies;
    public float spawnRateInterval;
    public GameObject[] typeOfEnemies;
}

public class enemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] enemySpawnPoints;
    private int currentWaveNum;

    private bool canSpawn = true;
    private bool canSpawnWave = true;
    private float nextSpawnTime;
    private Wave currentWave;
    [SerializeField] bool turnOnTimer;
    [SerializeField] float waveTime;
    private float waveTimeMax;
    private bool countWave = true;
    private void Start()
    {
        waveTimeMax = waveTime;
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
        //If Set Waves are Zero, Don't Spawn
        if (waves.Length == 0)
        {
            canSpawnWave = false;
        }

        //Else Spawn
        else if (waves.Length > 0 && canSpawnWave)
        {
            currentWave = waves[currentWaveNum];
            gameManager.instance.waveNameText.text = currentWave.waveName.ToString();
            if (countWave)
            {
                gameManager.instance.EnemiesInWaveCount = currentWave.numberOfEnemies;
            }
            countWave = false;
            SpawnWave();
        }
        //If All Enemies in Wave are Killed, Spawn Next Wave
        if (gameManager.instance.EnemiesInWaveCount == 0 && !canSpawn && currentWaveNum + 1 != waves.Length && gameManager.instance.TotalEnemyCount > 0  && canSpawnWave)
        {
            Debug.Log("Spawning Next Wave");
            countWave = true;
            waveTimer();

        }
    }

    public void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time && !gameManager.instance.isPaused)
        {
            GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            Transform randomPosition = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
            Instantiate(randomEnemy, randomPosition.position, transform.rotation);

            currentWave.numberOfEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnRateInterval;

            if (currentWave.numberOfEnemies == 0)
            {
                canSpawn = false;
            }
        }

    }

    private void waveTimer()
    {
        if (turnOnTimer)
        {
            gameManager.instance.BetweenWaveTimer = waveTime;
            waveTime -= Time.deltaTime;
            Debug.Log(waveTime);
            if (waveTime < 0f)
            {
                currentWaveNum++;
                canSpawn = true;
                waveTime = waveTimeMax;
                turnOnTimer = false;
            }
        }
    }
}


