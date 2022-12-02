using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    private int xPosition;
    private int yPosition;
    private int zPosition;
    [Header("-----Enemy Position Ranges-----")]
    [SerializeField] int xPosMinRange;
    [SerializeField] int xPosMaxRange;
    [SerializeField] int yPosMinRange;
    [SerializeField] int yPosMaxRange;
    [SerializeField] int zPosMinRange;
    [SerializeField] int zPosMaxRange;

    [Header("-----Enemy Spawning Variables-----")]
    [SerializeField] float spawnRate;
    [SerializeField] int maxEnemies;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (gameManager.instance.enemyCount < maxEnemies)
        {
            xPosition = Random.Range(xPosMinRange, xPosMaxRange);
            yPosition = Random.Range(yPosMinRange, xPosMaxRange);
            zPosition = Random.Range(zPosMinRange, zPosMaxRange);
            Instantiate(gameManager.instance.enemy, new Vector3(xPosition, yPosition, zPosition), Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
            gameManager.instance.enemyCount++;
        }

    }
}
