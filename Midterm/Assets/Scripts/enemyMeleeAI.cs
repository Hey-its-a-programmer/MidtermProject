using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyMeleeAI : MonoBehaviour, IDamage, IEffectable
{
    [Header("-----Components-----")]
    //Used for one or more model renderers
    [SerializeField] Renderer[] modelParts;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [Header("-----Enemy Stats-----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int coinValueMin;
    [SerializeField] int coinValueMax;
    [SerializeField] int sightAngle;
    [SerializeField] Transform headPos;
    [SerializeField] Transform dropSpawnPos;

    [Header("----- Enemy Melee Stats-----")]
    [SerializeField] float hitDelay;
    [SerializeField] float attackAngle;
    [SerializeField] GameObject meleeWeapon;
    //Only Use for things not part of the main model renderer
    [SerializeField] GameObject[] meleeWeaponParts;
    /*
    [Header("-------Drops-------")]
    [SerializeField] GameObject money;
    [Range(0, 100)] [SerializeField] float moneyDropChance;
    [SerializeField] float moneyDespawnTimer;
    [SerializeField] GameObject ammo;
    [Range(0, 100)] [SerializeField] float ammoDropChance;
    [SerializeField] float ammoDespawnTimer;
    */

    [Header("-------Enemy Animation-------")]
    [SerializeField] float deathFadeOutTime;
    [SerializeField] float deathAnimationTime;

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


    //Status Effect
    private StatusEffect _data;
    private bool isAttacking;
    private Vector3 playerDir;
    private bool isMoving;
    private float distance;
    private float angleToPlayer;
    private bool isAlive = true;
    private Transform test;
    private float moveSpeed;
    Renderer[] weaponRenderer;
    // Start is called before the first frame update
    void Start()
    {

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

        distance = Vector3.Distance(gameManager.instance.player.transform.position, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                facePlayer();
                if (!isAttacking && angleToPlayer <= attackAngle && distance <= agent.stoppingDistance + 1)
                {
                    StartCoroutine(attack());
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

        if (HP <= 0)
        {
            Death();
        }
    }
    /*
    void Drop()
    {
        // 50% chance to drop money
        if (Random.Range(0.0f, 100.0f) >= moneyDropChance)
        {

            GameObject cash = Instantiate(money, dropSpawnPos.position + new Vector3(0.0f, 1.0f, 0.0f), dropSpawnPos.rotation);

            Destroy(cash, moneyDespawnTimer);
        }

        // 80% chance to drop ammo
        if (Random.Range(0.0f, 100.0f) <= ammoDropChance)
        {
            GameObject ammunition = Instantiate(ammo, dropSpawnPos.position + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
            Destroy(ammunition, ammoDespawnTimer);
        }
    }
    */
    IEnumerator flashDamage()
    {
        // plays grunt noise to signal that the enemy took damage
        enemyAud.PlayOneShot(enemyHurtAudio[Random.Range(0, enemyHurtAudio.Length - 1)], enemyHurtVolume);
        for (int i = 0; i < modelParts.Length; i++)
        {
            modelParts[i].material.color = Color.red;
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < modelParts.Length; i++)
        {
            modelParts[i].material.color = Color.white;
        }
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
        yield return null;
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
        if (_data.DamageOverTimeAmount != 0 && CurrentEffectTime > nextTickTime)
        {
            nextTickTime += _data.TickSpeed;
            HP -= _data.DamageOverTimeAmount;
            if (HP <= 0)
            {
                Death();
            }
        }
    }

    void Death()
    {
        attackExit();
        agent.speed = 0;
        agent.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Ignore Collision");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;       
        isAlive = false;
        StartCoroutine(DeathAnimation());
        gameManager.instance.EnemiesInWaveCount--;
        gameManager.instance.updateTotalEnemyCount(-1);
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

    IEnumerator DeathFadeOut()
    {
        SetToFade();
        for (float t = 0.0f; t < deathFadeOutTime; t+= Time.deltaTime)
        {
            for (int i = 0; i < modelParts.Length; i++)
            {
                modelParts[i].material.color = new Color(modelParts[i].material.color.r, modelParts[i].material.color.g, modelParts[i].material.color.b, Mathf.Lerp(modelParts[i].material.color.a, 0, t));
            }

            for (int i = 0; i < meleeWeaponParts.Length; i++)
            {
                meleeWeaponParts[i].GetComponent<MeshRenderer>().material.color = new Color(meleeWeaponParts[i].GetComponent<MeshRenderer>().material.color.r, meleeWeaponParts[i].GetComponent<MeshRenderer>().material.color.g, meleeWeaponParts[i].GetComponent<MeshRenderer>().material.color.b, Mathf.Lerp(meleeWeaponParts[i].GetComponent<MeshRenderer>().material.color.a, 0, t));
            }
            
            meleeWeapon.GetComponent<MeshRenderer>().material.color = new Color(meleeWeapon.GetComponent<MeshRenderer>().material.color.r, meleeWeapon.GetComponent<MeshRenderer>().material.color.g, meleeWeapon.GetComponent<MeshRenderer>().material.color.b, Mathf.Lerp(meleeWeapon.GetComponent<MeshRenderer>().material.color.a, 0, t));
            yield return null;
        }
    }

    void SetToFade()
    {
        for (int i = 0; i < meleeWeaponParts.Length; i++)
        {
            meleeWeaponParts[i].GetComponent<MeshRenderer>().material.SetOverrideTag("RenderType", "Transparent");
            meleeWeaponParts[i].GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            meleeWeaponParts[i].GetComponent<MeshRenderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            meleeWeaponParts[i].GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 0);
            meleeWeaponParts[i].GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
            meleeWeaponParts[i].GetComponent<MeshRenderer>().material.EnableKeyword("_ALPHABLEND_ON");
            meleeWeaponParts[i].GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            meleeWeaponParts[i].GetComponent<MeshRenderer>().material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        meleeWeapon.GetComponent<MeshRenderer>().material.SetOverrideTag("RenderType", "Transparent");
        meleeWeapon.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        meleeWeapon.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        meleeWeapon.GetComponent<MeshRenderer>().material.SetInt("_ZWrite", 0);
        meleeWeapon.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHATEST_ON");
        meleeWeapon.GetComponent<MeshRenderer>().material.EnableKeyword("_ALPHABLEND_ON");
        meleeWeapon.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        meleeWeapon.GetComponent<MeshRenderer>().material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        for (int i = 0; i < modelParts.Length; i++)
        {
            modelParts[i].material.SetOverrideTag("RenderType", "Transparent");
            modelParts[i].material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            modelParts[i].material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            modelParts[i].material.SetInt("_ZWrite", 0);
            modelParts[i].material.DisableKeyword("_ALPHATEST_ON");
            modelParts[i].material.EnableKeyword("_ALPHABLEND_ON");
            modelParts[i].material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            modelParts[i].material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

    }

}
