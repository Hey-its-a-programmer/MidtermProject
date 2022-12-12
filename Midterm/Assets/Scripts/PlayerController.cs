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

    [Header("-----Gun Stats-----")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] GameObject gunModel;

    int selectedGun;
    bool isShooting;
    bool isSprinting;
    float speedOrig;


    int timesJumped;
    private Vector3 playerVelocity;
    Vector3 move;
    int HPOrig;
    Vector3 pushBack;
    private void Start()
    {
        speedOrig = playerSpeed;
        HPOrig = HP;
        setPlayerPos();
    }

    void Update()
    {
        controller.enabled = true;
        if (!gameManager.instance.isPaused)
        {
            pushBack = Vector3.Lerp(new Vector3(pushBack.x, 0, pushBack.z), Vector3.zero, Time.deltaTime * pushBackTime);
            movement();

            playerSprint();
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
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move((playerVelocity + pushBack)* Time.deltaTime);
    }

    IEnumerator shoot()
    {
        if (!isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;

            //Instantiate(cube, transform.position, transform.rotation);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                //Instantiate(cube, hit.point, transform.rotation);
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
            }


            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(playerDamageFlash());
        if (HP <= 0)
        {
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

    public void gunPickup(gunStats gunStat)
    {
        shootRate = gunStat.shootRate;
        shootDamage = gunStat.shootDamage;
        shootDist = gunStat.shootDist;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunStat.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStat.gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        gunList.Add(gunStat);
        selectedGun = gunList.Count - 1;
    }

    public List<gunStats> getGunList()
    {
        return gunList;
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

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void pushBackInput(Vector3 direction)
    {
        pushBack = direction;
    }
}