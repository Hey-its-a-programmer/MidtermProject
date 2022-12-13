using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Image playerHPBar;
    [SerializeField] TextMeshProUGUI enemyRemaining;


    [Header("----- Enemy Waves-----")]

    private int totalEnemyCount;
    private int enemiesInWaveCount;

    //public enemyAI enemyScript;
    //public GameObject enemy;
    [Header("----- Other Functions -----")]
    public bool isPaused;
    float timeScaleOrig;
    public GameObject playerSpawnPos;
    public bool turnCameraOn;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        //enemyScript = enemy.GetComponent<enemyAI>();
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
        TotalEnemyCount += amount;
        if (TotalEnemyCount <= 0)
        {
            winMenu.SetActive(true);
            pause();
            activeMenu = winMenu;
        }
    }

    public int EnemiesInWaveCount
    {
        get { return enemiesInWaveCount; }
        set { enemiesInWaveCount = value; }
    }


    public int TotalEnemyCount
    {
        get {return totalEnemyCount;}
        set {totalEnemyCount = value;}

    }
    public void updateEnemyCount(int amount)
    {
        enemiesInWaveCount += amount;
        enemyRemaining.text = enemiesInWaveCount.ToString("F0");
        if (enemiesInWaveCount <= 0)
        {
            winMenu.SetActive(true);
            pause();
            activeMenu = winMenu;
        }
    }
}

