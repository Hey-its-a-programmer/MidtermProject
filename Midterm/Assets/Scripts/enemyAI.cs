using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class enemyAI : MonoBehaviour, IDamage, IEffectable
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
    //[SerializeField] int sightAngle;
    [SerializeField] Transform headPos;
    [SerializeField] Transform dropSpawnPos;
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
[Header("-------Drops-------")]
    [SerializeField] GameObject money;
    [SerializeField] GameObject ammo;//Status Effect
    private StatusEffect _data;    int HPOrg;
    private float _currentMoveSpeed;
    float moveSpeed;
    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    bool isMoving;
    float angleToPlayer;
  
    // Start is called before the first frame update
    void Start()
    {
        HPOrg = HP;
        moveSpeed = agent.speed;
        //_currentMoveSpeed = moveSpeed;
        
        //AJ changes
        updateEnemyHPBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (_data != null) HandleEffect();
        anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
        if (playerInRange)
        {
            canSeePlayer();
            agent.SetDestination(gameManager.instance.player.transform.position);
            if (!isMoving && agent.velocity.magnitude > 0.5f && agent.isStopped == false)
            {
                // if the enemy is standing still, this sound won't play
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

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {

            if (hit.collider.CompareTag("Player") /*&& angleToPlayer <= sightAngle*/)
            {
                if (!isShooting && angleToPlayer <= 15)
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
        UI.gameObject.SetActive(true);
        
        if (HP <= 0)
        {
            Destroy(gameObject);
            Drop();
            gameManager.instance.EnemiesInWaveCount--;
            gameManager.instance.updateTotalEnemyCount(-1);
        }
    }

    void Drop()
    {
        // 50% chance to drop money
        if (Random.Range(0.0f, 100.0f) >= 50.0f)
        {
            Vector3 dropPos = dropSpawnPos.position;
            GameObject cash = Instantiate(money, dropPos + new Vector3(0.0f, 1.0f, 0.0f), dropSpawnPos.rotation);
            cash.SetActive(true);
            Destroy(cash, 5.0f);
        }

        // 80% chance to drop ammo
        if (Random.Range(0.0f, 100.0f) <= 80.0f)
        {
            Vector3 dropPos = dropSpawnPos.position;
            GameObject ammunition = Instantiate(ammo, dropPos + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
            ammunition.SetActive(true);
            Destroy(ammunition, 5.0f);
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
        /*enemyAud.PlayOneShot(enemyStepAudio[Random.Range(0, enemyStepAudio.Length - 1)], enemyStepVolume);*/

        yield return new WaitForSeconds(0.5f);

        isMoving = false;
    }
    /*public void enemyHP()
    {
        enemyHPbar.fillAmount = (float)HP / (float)HPOrg;

    }*/

    public void updateEnemyHPBar()
    {
        enemyHPbar.fillAmount = (float)HP / (float)HPOrg;
    }

    //----Status Effect Methods
    
    private GameObject _effectParticles;

    private float CurrentEffectTime = 0f;
    private float nextTickTime = 0f;

    public void ApplyEffect(StatusEffect _data)
    {
        RemoveEffect();
        this._data = _data;
        agent.speed = moveSpeed / _data.MovementPentalty;
        _effectParticles = Instantiate(_data.EffectParticles, transform);
    }
    public void RemoveEffect()
    {
        _data = null;
        CurrentEffectTime = 0;
        nextTickTime = 0;
        agent.speed = moveSpeed;
        if (_effectParticles != null)
        {
            Destroy(_effectParticles);
        }
    }
    public void HandleEffect()
    {
        CurrentEffectTime += Time.deltaTime;

        if (CurrentEffectTime >= +_data.Lifetime)
        {
            RemoveEffect();
        }
        if (_data == null)
        {
            return;
        }
        if (_data.DamageOverTimeAmount != 0 && CurrentEffectTime > nextTickTime )
        {
            nextTickTime += _data.TickSpeed;
            HP -= _data.DamageOverTimeAmount;
            if (HP <= 0)
            {
                gameManager.instance.EnemiesInWaveCount--;
                gameManager.instance.updateTotalEnemyCount(-1);
                gameManager.instance.playerScript.coins += Random.Range(coinValueMin, coinValueMax);
                Destroy(gameObject);
            }
        }
    }
}
