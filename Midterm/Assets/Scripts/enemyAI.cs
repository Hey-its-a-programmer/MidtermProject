using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{

    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("-----Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightAngle;
    [SerializeField] Transform headPos;

    [Header("----- Enemy Gun Stats-----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    [Header("-------Enemy Audio-------")]
    [SerializeField] AudioSource enemyAud;
    [SerializeField] AudioClip gunShotClip;
    [Range(0, 1)] [SerializeField] float gunShotVolume;
    [SerializeField] AudioClip[] enemyHurtAudio;
    [Range(0, 1)] [SerializeField] float enemyHurtVolume;
    [SerializeField] AudioClip[] enemyStepAudio;
    [Range(0, 1)] [SerializeField] float enemyStepVolume;
    [SerializeField] AudioClip[] enemyDeathAudio;
    [Range(0, 1)] [SerializeField] float enemyDeathVolume;
    

    int HPOrg;
    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        HPOrg = HP;
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
                StartCoroutine(EnemySteps());
            }
        }
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

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                if (!isShooting)
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

    public int getEnemyHealth()
    {
        return HP;
    }

    public void setEnemyHealth(int setHealth)
    {
        HP = setHealth;
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            StartCoroutine(EnemyDied());
            gameManager.instance.updateTotalEnemyCount(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashDamage()
    {
        enemyAud.PlayOneShot(enemyHurtAudio[Random.Range(0, enemyHurtAudio.Length - 1)], enemyHurtVolume);
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootPos.position, transform.rotation);

        enemyAud.PlayOneShot(gunShotClip, gunShotVolume);

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    IEnumerator EnemySteps()
    {
        isMoving = true;

        enemyAud.PlayOneShot(enemyStepAudio[Random.Range(0, enemyStepAudio.Length - 1)], enemyStepVolume);

        yield return new WaitForSeconds(0.5f);

        isMoving = false;
    }

    IEnumerator EnemyDied()
    {
        enemyAud.PlayOneShot(enemyDeathAudio[Random.Range(0, enemyDeathAudio.Length - 1)], enemyDeathVolume);
        yield return null;
    }
}
