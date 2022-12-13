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
    //[SerializeField] int sightAngle;
    [SerializeField] Transform headPos;
    [SerializeField] int pushBackTime;
    [SerializeField] Vector3 enemyVelocity;
    [Header("----- Enemy Gun Stats-----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    [Header("----- Enemy ui-----")]
    [SerializeField] Image enemyHPbar;
    [SerializeField] GameObject UI;

    int HPOrg;
    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    //float angleToPlayer;
    Vector3 pushBack;
    Vector3 enemyMovement;
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
            enemyVelocity.y = 0.0f;
            agent.Move((enemyVelocity + pushBack) * Time.deltaTime);
        }

        if (!gameManager.instance.isPaused)
        {
            pushBack = Vector3.Lerp(new Vector3(pushBack.x, 0, pushBack.z), Vector3.zero, Time.deltaTime * pushBackTime);
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
        //angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") /*&& angleToPlayer <= sightAngle*/)
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
        agent.SetDestination(gameManager.instance.player.transform.position);
        HP -= dmg;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            gameManager.instance.EnemiesInWaveCount--;
            gameManager.instance.updateTotalEnemyCount(-1);
            Destroy(gameObject);

        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }
<<<<<<< HEAD
    public void enemyHP()
    {
        enemyHPbar.fillAmount = (float)HP / (float)HPOrg;
=======
    }
    public void pushBackInput(Vector3 direction)
    {
        pushBack = direction;
>>>>>>> 63f2536df7f7f232164ee22e71d262bc26746e79
    }
}
