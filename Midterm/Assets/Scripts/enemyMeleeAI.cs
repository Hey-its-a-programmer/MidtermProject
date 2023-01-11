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
    [SerializeField] float attackAngle;
    [SerializeField] GameObject meleeWeapon;

    [Header("----- Enemy UI-----")]
    //UI Needs To Be Fixed For HP Bars To Work
    //[SerializeField] Image enemyHPBar;
    //[SerializeField] GameObject UI;

    [Header("-------Enemy Audio-------")]
    [SerializeField] AudioSource enemyAud;

    //Needs to be changed for melee attack.
    [SerializeField] AudioClip meleeAttackAudtio;
    [Range(0, 1)] [SerializeField] public float meleeAttackVolume;

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
    private float distance;

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

        distance = Vector3.Distance(gameManager.instance.player.transform.position, transform.position);
        //Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") /*&& angleToPlayer <= sightAngle*/)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                //transform.LookAt(gameManager.instance.player.transform.position);
                facePlayer();
                if (!isAttacking && angleToPlayer <= attackAngle && distance <= agent.stoppingDistance)
                {
                    //Debug.Log("Attacking");
                    StartCoroutine(attack());
                    /*
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        facePlayer();
                    }
                    */
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
        enemyAud.PlayOneShot(enemyHurtAudio[Random.Range(0, enemyHurtAudio.Length - 1)], enemyHurtVolume);
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }
    
    void attackEntry()
    {
        meleeWeapon.GetComponent<BoxCollider>().isTrigger = true;
        // Needs Melee sounds instead
        enemyAud.PlayOneShot(meleeAttackAudtio, meleeAttackVolume);
    }

    void attackExit()
    {
        meleeWeapon.GetComponent<BoxCollider>().isTrigger = false;
    }

    IEnumerator attack()
    {
        isAttacking = true;
        anim.SetTrigger("Swing");
        attackEntry();
        yield return new WaitForSeconds(hitRate);
        attackExit();
        isAttacking = false;
    }

    IEnumerator EnemySteps()
    {
        isMoving = true;
        //plays footsteps of enemy
        enemyAud.PlayOneShot(enemyStepAudio[Random.Range(0, enemyStepAudio.Length - 1)], enemyStepVolume);

        yield return new WaitForSeconds(0.5f);

        isMoving = false;
    }

    /*
    public void updateEnemyHPBar()
    {
        enemyHPBar.fillAmount = (float)HP / (float)HPOrg;
    }
    */
}
