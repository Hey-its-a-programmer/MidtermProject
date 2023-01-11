using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyMeleeAI : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [Header("-----Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int coinValueMin;
    [SerializeField] int coinValueMax;
    [SerializeField] int sightAngle;
    [SerializeField] Transform headPos;

    [Header("----- Enemy Melee Stats-----")]
    [SerializeField] float hitRate;
    [SerializeField] float attackRange;
    [SerializeField] GameObject meleeWeapon;

    [Header("----- Enemy UI-----")]
    //[SerializeField] Image enemyHPBar;
    //[SerializeField] GameObject UI;

    [Header("-------Enemy Audio-------")]
    //[SerializeField] AudioSource enemyAud;

    //Needs to be changed for melee attack.
    //[SerializeField] AudioClip gunShotClip;
    [Range(0, 1)] [SerializeField] public float gunShotVolume;

    // sounds for when enemy is damaged
    [SerializeField] AudioClip[] enemyHurtAudio;
    [Range(0, 1)] [SerializeField] public float enemyHurtVolume;

    //sounds for when enemy is walking
    [SerializeField] AudioClip[] enemyStepAudio;
    [Range(0, 1)] [SerializeField] public float enemyStepVolume;


    private int HPOrg;
    private bool isAttacking;
    private Vector3 playerDir;
    private bool isMoving;


    private float angleToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        HPOrg = HP;
        //updateEnemyHPBar();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

        canSeePlayer();
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (!isMoving && agent.velocity.magnitude > 0.5f && agent.isStopped == false)
        {
            StartCoroutine(EnemySteps());
        }

        
    }
    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }

    void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;

        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        //Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") /*&& angleToPlayer <= sightAngle*/)
            {
                if (!isAttacking && angleToPlayer <= 15 /*&& distance <= attackRange*/)
                {
                    Debug.Log("Attacking");
                    StartCoroutine(attack());

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
        //updateEnemyHPBar();
        StartCoroutine(flashDamage());
        //UI.gameObject.SetActive(true);

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
        //enemyAud.PlayOneShot(enemyHurtAudio[Random.Range(0, enemyHurtAudio.Length - 1)], enemyHurtVolume);
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }

    IEnumerator attack()
    {
        isAttacking = true;
        anim.SetTrigger("Swing");
        
        // Needs Melee sounds instead
        //enemyAud.PlayOneShot(gunShotClip, gunShotVolume);

        yield return new WaitForSeconds(hitRate);

        isAttacking = false;
    }

    IEnumerator EnemySteps()
    {
        isMoving = true;
        //plays footsteps of enemy
        //enemyAud.PlayOneShot(enemyStepAudio[Random.Range(0, enemyStepAudio.Length - 1)], enemyStepVolume);

        yield return new WaitForSeconds(0.5f);

        isMoving = false;
    }

    /*
    public void updateEnemyHPBar()
    {
        enemyHPBar.fillAmount = (float)HP / (float)HPOrg;
    }
    */

    //public void pushBackInput(Vector3 direction)
    //{
    //    pushBack = direction;

    //}
}
