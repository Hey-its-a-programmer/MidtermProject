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


    [Header("----- Enemy Waves-----")]
    public Wave[] waves;
    private int enemyTotalCount;
    private int enemiesInWaveCount;
    public Transform[] enemySpawnPoints;
    public int currentWaveNum;
    public enemyAI enemyScript;
    public GameObject enemy;
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
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyScript = enemy.GetComponent<enemyAI>();
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

