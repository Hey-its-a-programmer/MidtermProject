using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    //X Y Z Positions for Enemies to spawn
    private int xPosition;
    private int yPosition;
    private int zPosition;

    //Ranges for Positions of Enemies to spawn at
    [Header("-----Enemy Position Ranges-----")]
    [SerializeField] int xPosMinRange;
    [SerializeField] int xPosMaxRange;
    [SerializeField] int yPosMinRange;
    [SerializeField] int yPosMaxRange;
    [SerializeField] int zPosMinRange;
    [SerializeField] int zPosMaxRange;

    //The Spawn Rate of the enemies and the Maximum Amount of Enemies
    [Header("-----Enemy Spawning Variables-----")]
    [SerializeField] float spawnRate;
    [SerializeField] int maxEnemies;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    //Spawns Enemies within set paramaters
    //Spawns them in a field randomly
    IEnumerator SpawnEnemy()
    {
        gameManager.instance.updateEnemyCount(maxEnemies);
        while (gameManager.instance.enemyCount < maxEnemies)
        {
            xPosition = Random.Range(xPosMinRange, xPosMaxRange);
            yPosition = Random.Range(yPosMinRange, xPosMaxRange);
            zPosition = Random.Range(zPosMinRange, zPosMaxRange);
            Instantiate(gameManager.instance.enemy, new Vector3(xPosition, yPosition, zPosition), Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
        }

    }
}
