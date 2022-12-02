using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int numberOfEnemies;
    public GameObject Enemy;
    public float spawnRateInterval;
}

public class enemySpawner : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    public GameObject[] spawnPoints;

    private void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Points");
    }
}
