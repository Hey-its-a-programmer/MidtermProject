using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class enemyAI : MonoBehaviour, IDamage
{

    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("-----Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int coinValueMin;
    [SerializeField] int coinValueMax;
    //[SerializeField] int sightAngle;
    [SerializeField] Transform headPos;
    //[SerializeField] int pushBackTime;
    //[SerializeField] Vector3 enemyVelocity;
    [Header("----- Enemy Gun Stats-----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    [Header("----- Enemy UI-----")]
    [SerializeField] Image enemyHPbar;
    [SerializeField] GameObject UI;

    [Header("-------Enemy Audio-------")]
    [SerializeField] AudioSource enemyAud;

    //sound for when enemy shoots
    [SerializeField] AudioClip gunShotClip;
    [Range(0, 1)] [SerializeField] public float gunShotVolume;

    // sounds for when enemy is damaged
    [SerializeField] AudioClip[] enemyHurtAudio;
    [Range(0, 1)] [SerializeField] public float enemyHurtVolume;

    //sounds for when enemy is walking
    [SerializeField] AudioClip[] enemyStepAudio;
    [Range(0, 1)] [SerializeField] public float enemyStepVolume;   
    int HPOrg;
    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    bool isMoving;
    //AJ changes
    float angleToPlayer;
    //


    //Vector3 pushBack;
    //Vector3 enemyMovement;    
    // Start is called before the first frame update
    void Start()
    {
        HPOrg = HP;
        //AJ changes
        updateEnemyHPBar();
        //
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            canSeePlayer();
            agent.SetDestination(gameManager.instance.player.transform.position);
            if (!isMoving && agent.velocity.magnitude > 0.5f && agent.isStopped == false)
            {
                // if the enemy is standing still, this sound won't play
                StartCoroutine(EnemySteps());
            }
            //enemyVelocity.y = 0.0f;
           // agent.Move((enemyVelocity + pushBack) * Time.deltaTime);
        }
        
        //if (!gameManager.instance.isPaused)
        //{
        //    pushBack = Vector3.Lerp(new Vector3(pushBack.x, 0, pushBack.z), Vector3.zero, Time.deltaTime * pushBackTime);
        //}
    }
    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void canSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position - headPos.position);

        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {

            if (hit.collider.CompareTag("Player") /*&& angleToPlayer <= sightAngle*/)
            {
                //change
                if (!isShooting && angleToPlayer <= 15)
                //
                {
                    StartCoroutine(shoot());

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        facePlayer();
                    }
                }
            }
        }
    }

    public void takeDamage(int dmg)
    {

        HP -= dmg;
        updateEnemyHPBar();
        StartCoroutine(flashDamage());
        //change
        UI.gameObject.SetActive(true);
        //

        if (HP <= 0)
        {
            gameManager.instance.EnemiesInWaveCount--;
            gameManager.instance.updateTotalEnemyCount(-1);
            gameManager.instance.playerScript.coins += Random.Range(coinValueMin, coinValueMax);
            Destroy(gameObject);
        }
    }

    IEnumerator flashDamage()
    {
        // plays grunt noise to signal that the enemy took damage
        enemyAud.PlayOneShot(enemyHurtAudio[Random.Range(0, enemyHurtAudio.Length - 1)], enemyHurtVolume);
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootPos.position, transform.rotation);

        // same gunshot noise as player for now
        enemyAud.PlayOneShot(gunShotClip, gunShotVolume);

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    IEnumerator EnemySteps()
    {
        isMoving = true;

        //plays footsteps of enemy
        enemyAud.PlayOneShot(enemyStepAudio[Random.Range(0, enemyStepAudio.Length - 1)], enemyStepVolume);

        yield return new WaitForSeconds(0.5f);

        isMoving = false;
    }
    /*public void enemyHP()
    {
        enemyHPbar.fillAmount = (float)HP / (float)HPOrg;

    }*/

    //change
    public void updateEnemyHPBar()
    {
        enemyHPbar.fillAmount = (float)HP / (float)HPOrg;
    }
    //


    //public void pushBackInput(Vector3 direction)
    //{
    //    pushBack = direction;

    //}
}
