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
    public enemyAI enemyScript;

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

 

    [Header("-------Game Audio-------")]
    [SerializeField] public AudioSource gameManagerAud;
    [Range(0, 1)] [SerializeField] public float masterVolume;

    // fanfare for when player wins, game over for when player loses
    [SerializeField] AudioClip winMusic;
    [Range(0, 1)] [SerializeField] float winMusicVolume;
    [SerializeField] public AudioClip loseMusic;
    [Range(0, 1)] [SerializeField] public float loseMusicVolume;

    // sounds for when the game is paused or unpaused
    [SerializeField] public AudioClip pauseSound;
    [Range(0, 1)] [SerializeField] public float pauseSoundVolume;
    [SerializeField] public AudioClip unpauseSound;
    [Range(0, 1)] [SerializeField] public float unpauseSoundVolume;

    // sounds for hitting wall or enemies
    [SerializeField] public AudioClip[] hitWallAudio;
    [Range(0, 1)] [SerializeField] public float hitWallVolume;
    [SerializeField] public AudioClip hitEnemyAudio;
    [Range(0, 1)] [SerializeField] public float hitEnemyVolume;


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
        masterVolume = AudioListener.volume;
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        //enemyScript = enemy.GetComponent<enemyAI>();
        timeScaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVolume();
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
            {
                // plays pause sound
                gameManagerAud.PlayOneShot(pauseSound, pauseSoundVolume);
                pause();
            }
            else
            {
                // plays unpause sound
                gameManagerAud.PlayOneShot(unpauseSound, unpauseSoundVolume);
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
            //plays fanfare for winning
            gameManagerAud.PlayOneShot(winMusic, winMusicVolume);

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

    void UpdateVolume()
    {
        AudioListener.volume = masterVolume;
    }
}

