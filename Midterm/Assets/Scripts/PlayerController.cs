using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;

    [Header("-----Player Stats-----")]
    [SerializeField] int HP;

    [Range(3, 8)] [SerializeField] float playerSpeed;
    [Range(1, 5)] [SerializeField] float sprintSpeed;
    [Range(0, 15)] [SerializeField] int jumpHeight;
    [Range(15, 35)] [SerializeField] int gravityValue;
    [Range(0, 3)] [SerializeField] int jumpMax;
    [SerializeField] int pushBackTime;
    public int coins;
    [Header("-----Gun Stats-----")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;

    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject hitEffect;

    




    [Header("-------Player Audio-------")]

    //code from class
    [SerializeField] AudioSource playerAud;
    [SerializeField] AudioClip gunShotClip;
    [Range(0, 1)] [SerializeField] public float gunShotVolume;
    [SerializeField] AudioClip[] playerJumpAudio;
    [Range(0, 1)] [SerializeField] public float playerJumpVolume;

    // code from class
    [SerializeField] AudioClip[] playerStepAudio;
    [Range(0, 1)] [SerializeField] public float playerStepVolume;

    // sounds for when player is damaged
    [SerializeField] AudioClip[] playerHurtAudio;
    [Range(0, 1)] [SerializeField] public float playerHurtVolume;

    // sounds for when player dies
    [SerializeField] AudioClip[] playerDeathAudio;
    [Range(0, 1)] [SerializeField] public float playerDeathVolume;


    private bool isShooting;
    private bool isSprinting;
    private float speedOrig;
    private bool isMoving;
    private int maxAmmo;
    private int currentAmmo;
    private int timesJumped;
    private Vector3 playerVelocity;
    private Vector3 move;
    private int HPOrig;
    private int selectedGun;
    private Vector3 pushBack;
    private int coinsOriginal;
    

   

   
    private void Start()
    {
        controller.enabled = true;
        maxAmmo = gunList[0].maxAmmo;
        currentAmmo = maxAmmo;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[0].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[0].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        speedOrig = playerSpeed;
        HPOrig = HP;
        coinsOriginal = coins;
        setPlayerPos();

    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            pushBack = Vector3.Lerp(new Vector3(pushBack.x, 0, pushBack.z), Vector3.zero, Time.deltaTime * pushBackTime);
            movement();

            if (!isMoving && move.magnitude > 0.3f && controller.isGrounded)
            {
                StartCoroutine(PlayerSteps());
            }

            if (gunList.Count > 0)
            {
                StartCoroutine(shoot());
                gunSelect();
            }

        }
        
    }

    void movement()
    {
        
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumped = 0;
        }

        move = transform.right * Input.GetAxis("Horizontal") +
               transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * playerSpeed);



        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && timesJumped < jumpMax)
        {
            playerVelocity.y = jumpHeight;
            timesJumped++;
            playerAud.PlayOneShot(playerJumpAudio[Random.Range(0, playerJumpAudio.Length)], playerJumpVolume);
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack) * Time.deltaTime);
    }

    IEnumerator shoot()
    {

        if (!isShooting && Input.GetButton("Shoot") && currentAmmo > 0)
        {
            
            isShooting = true;
            currentAmmo--;//reduces ammo by -1
            Debug.Log("Shooting");
            playerAud.PlayOneShot(gunList[selectedGun].gunshot, gunShotVolume);

            //Instantiate(cube, transform.position, transform.rotation);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                
                //Instantiate(cube, hit.point, transform.rotation);
               
                if (hit.collider.GetComponent<IDamage>() != null )
                {
                    
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                    gameManager.instance.gameManagerAud.PlayOneShot(gameManager.instance.hitEnemyAudio, gameManager.instance.hitEnemyVolume);
                }
                else
                {
                    gameManager.instance.gameManagerAud.PlayOneShot(gameManager.instance.hitWallAudio[Random.Range(0, gameManager.instance.hitWallAudio.Length)], gameManager.instance.hitWallVolume);
                }

                

                Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
                yield return new WaitForSeconds(shootRate);
            }
            
            isShooting = false;
        }

       
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        playerAud.PlayOneShot(playerHurtAudio[Random.Range(0, playerHurtAudio.Length)], playerHurtVolume);

        updatePlayerHPbar();

        StartCoroutine(playerDamageFlash());
        if (HP <= 0)
        {
            controller.enabled = false;
            playerAud.PlayOneShot(playerDeathAudio[Random.Range(0, playerDeathAudio.Length)], playerDeathVolume);
            gameManager.instance.gameManagerAud.PlayOneShot(gameManager.instance.loseMusic, gameManager.instance.loseMusicVolume);
            gameManager.instance.pause();
            gameManager.instance.loseMenu.SetActive(true);
            gameManager.instance.activeMenu = gameManager.instance.loseMenu;
        }
    }

    IEnumerator playerDamageFlash()
    {
        gameManager.instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerFlashDamage.SetActive(false);

    }

    public void setPlayerPos()
    {
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void resetPlayerHP()
    {
        HP = HPOrig;

        updatePlayerHPbar();

    }

    public void resetPlayerCoins()
    {
        coins = coinsOriginal;
    }

    public void playerSprint()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {

            if (!isSprinting)
            {
                playerSpeed = playerSpeed + sprintSpeed;

                isSprinting = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerSpeed = speedOrig;
            isSprinting = false;
        }
    }

    IEnumerator PlayerSteps()
    {
        isMoving = true;

        //code from class
        playerAud.PlayOneShot(playerStepAudio[Random.Range(0, playerStepAudio.Length)], playerStepVolume);

        if (isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.4f);
        }

        isMoving = false;
    }
    public void gunPickup(gunStats gunStat)
    {
        shootRate = gunStat.shootRate;
        shootDamage = gunStat.shootDamage;
        shootDist = gunStat.shootDist;
        maxAmmo = gunStat.maxAmmo;
        currentAmmo = maxAmmo;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        gunList.Add(gunStat);
        selectedGun = gunList.Count - 1;
    }
    
    public void HealthPickup(MedkitStats medStat)
    {
        if (HP < HPOrig)
        {
            gameManager.instance.gameManagerAud.PlayOneShot(gameManager.instance.healthRestoreAudio);
            HP += medStat.restoredHP;
            if (HP > HPOrig)
            {
                HP = HPOrig;
            }
            updatePlayerHPbar();
        }
        else
        {
            gameObject.GetComponent<SphereCollider>().isTrigger = false;
        }
    }

    public void MoneyPickup(MoneyStats monStat)
    {
        gameManager.instance.gameManagerAud.PlayOneShot(gameManager.instance.moneyPickupAudio);
        coins += monStat.moneyGiven;
    }

    public void AmmoPickup(AmmoStats ammoStat)
    {
        if (currentAmmo < maxAmmo)
        {
            gameManager.instance.gameManagerAud.PlayOneShot(gameManager.instance.ammoRestoreAudio);
            currentAmmo += ammoStat.restoredAmmo;

            if (currentAmmo > maxAmmo)
            {
                currentAmmo = maxAmmo;
            }
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
   public List<gunStats> GunList
    {
        get { return gunList; }
        set { gunList = value; }
    }

    void gunSelect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }

        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootRate = gunList[selectedGun].shootRate;
        shootDist = gunList[selectedGun].shootDist;
        maxAmmo = gunList[selectedGun].maxAmmo;
        currentAmmo = maxAmmo;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void pushBackInput(Vector3 direction)
    {
        pushBack = direction;
    }


    public void updatePlayerHPbar()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)HPOrig;
    }

    

    public int CurrentAmmo
    {
        get { return currentAmmo; }
        set { currentAmmo = value; }
    }
}