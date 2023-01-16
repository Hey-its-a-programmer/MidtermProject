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
    [SerializeField] Transform headPos;
    [SerializeField] Transform dropSpawnPos;
    [Header("----- Enemy Gun Stats-----")]
    [SerializeField] float shootRate;
    [SerializeField] float shootAngle;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    [Header("-------Drops-------")]
    [SerializeField] GameObject money;
    [Range(0, 100)] [SerializeField] float moneyDropChance;
    [SerializeField] float moneyDespawnTimer;
    [SerializeField] GameObject ammo;
    [Range(0, 100)] [SerializeField] float ammoDropChance;
    [SerializeField] float ammoDespawnTimer;

    [Header("-------Enemy Animation-------")]
    [SerializeField] float deathFadeOutTime;
    [SerializeField] float deathAnimationTime;

    [Header("----- Enemy UI-----")]
    [SerializeField] Image enemyHPbar;
    [SerializeField] GameObject UI;

    [Header("-------Enemy Audio-------")]
    [SerializeField] AudioSource enemyAud;

    //sound for when enemy shoots
    [SerializeField] AudioClip gunShotClip;

    [Range(0, 1)] public float gunShotVolume;

    // sounds for when enemy is damaged
    [SerializeField] AudioClip[] enemyHurtAudio;

    [Range(0, 1)] public float enemyHurtVolume;

    //sounds for when enemy is walking
    [SerializeField] AudioClip[] enemyStepAudio;
    [Range(0, 1)] public float enemyStepVolume;

    //Status Effect
    private StatusEffect _data;
    private float _currentMoveSpeed;
    float moveSpeed;
    bool isShooting;
    private bool isAlive = true;
    Vector3 playerDir;
    bool isMoving;
    float angleToPlayer;
  
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = agent.speed;
        //_currentMoveSpeed = moveSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isAlive)
        {
            if (_data != null)
            {
                HandleEffect();
            }
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);


            canSeePlayer();
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


    void canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;

        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {

            if (hit.collider.CompareTag("Player"))
            {

                agent.SetDestination(gameManager.instance.player.transform.position);
                facePlayer();
                if (!isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());

                }
            }
        }
    }

    public void takeDamage(int dmg)
    {
        if (isAlive)
        {
            HP -= dmg;
            StartCoroutine(flashDamage());
        }

        if (HP <= 0 && isAlive)
        {
            agent.speed = 0;
            agent.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Ignore Collision");
            isAlive = false;
            StartCoroutine(DeathAnimation());
            Drop();
            gameManager.instance.EnemiesInWaveCount--;
            gameManager.instance.updateTotalEnemyCount(-1);
        }
    }

    void Drop()
    {
        // 50% chance to drop money
        if (Random.Range(0.0f, 100.0f) >= moneyDropChance)
        {
            Vector3 dropPos = dropSpawnPos.position;
            GameObject cash = Instantiate(money, dropSpawnPos.position + new Vector3(0.0f, 1.0f, 0.0f), dropSpawnPos.rotation);
            cash.SetActive(true);
            Destroy(cash, moneyDespawnTimer);
        }

        // 80% chance to drop ammo
        if (Random.Range(0.0f, 100.0f) <= ammoDropChance)
        {
            GameObject ammunition = Instantiate(ammo, dropSpawnPos.position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
            Destroy(ammunition, ammoDespawnTimer);
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
        anim.SetTrigger("Shoot");
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

                Drop();
                StartCoroutine(DeathAnimation());
            }
        }
    }

    /*
    * The corutines and setToFade all deal with death animations and fading out
    */
    IEnumerator DeathAnimation()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(deathAnimationTime);
        StartCoroutine(DeathFadeOut());
    }

    //Make weaponRenderer a thing (most likely a GameObject) if we are going to use another model for the gun
    IEnumerator DeathFadeOut()
    {
        SetToFade();
        for (float t = 0.0f; t < deathFadeOutTime; t += Time.deltaTime)
        {
            model.material.color = new Color(model.material.color.r, model.material.color.g, model.material.color.b, Mathf.Lerp(model.material.color.a, 0, t));
            /*
            weaponRenderer.material.color = new Color(weaponRenderer.material.color.r, weaponRenderer.material.color.g, weaponRenderer.material.color.b, Mathf.Lerp(weaponRenderer.material.color.a, 0, t));
            */
            yield return null;
        }

        Destroy(gameObject);
    }

    void SetToFade()
    {
        /*
        weaponRenderer.material.SetOverrideTag("RenderType", "Transparent");
        weaponRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        weaponRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        weaponRenderer.material.SetInt("_ZWrite", 0);
        weaponRenderer.material.DisableKeyword("_ALPHATEST_ON");
        weaponRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        weaponRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        weaponRenderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        */

        model.material.SetOverrideTag("RenderType", "Transparent");
        model.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        model.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        model.material.SetInt("_ZWrite", 0);
        model.material.DisableKeyword("_ALPHATEST_ON");
        model.material.EnableKeyword("_ALPHABLEND_ON");
        model.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        model.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}
