using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    [Header("----- Player Stuff -----")]
    public GameObject player;
    public PlayerController playerScript;
   
    [Header("----- UI Stuff -----")]
    public GameObject pauseMenu;
    public GameObject activeMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject playerFlashDamage;


    [Header("----- Collectibles -----")]
    public int jumpCost;
    public int coins;

    [Header("----- Enemy -----")]
    public GameObject enemy;
    private int enemyTotalCount;
    private int enemiesInWaveCount;

    [Header("----- Other Functions -----")]
    public bool isPaused;
    float timeScaleOrig;
    public GameObject playerSpawnPos;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        timeScaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
            if (isPaused)
            {
                pause();
            }
            else
            {
                unPause();
            }
        }
    }

    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unPause()
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }
    public void addCoins(int amount)
    {
        coins += amount;
        
    }

    public void updateTotalEnemyCount(int amount)
    {
        enemyTotalCount += amount;
        if (enemyTotalCount <= 0)
        {
            winMenu.SetActive(true);
            pause();
            activeMenu = winMenu;
        }
    }

    public void setEnemyWaveCounter(int amount)
    {
        enemiesInWaveCount += amount;
    }

    public int getEnemyWaveCount()
    {
        return enemiesInWaveCount;
    }

    public int getTotalEnemyCount()
    {
        return enemyTotalCount;
    }
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public int numberOfEnemies;
    public float spawnRateInterval;
    public GameObject Enemy;
}