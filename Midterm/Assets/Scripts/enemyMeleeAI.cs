using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyMeleeAI : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] MeshRenderer model;
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
    [SerializeField] float hitDelay;
    [SerializeField] float attackAngle;
    [SerializeField] GameObject meleeWeapon;

    [Header("----- Enemy UI-----")]
    //UI Needs To Be Fixed For HP Bars To Work
    //[SerializeField] Image enemyHPBar;
    //[SerializeField] GameObject UI;

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

    private int HPOrg;
    private bool isAttacking;
    private Vector3 playerDir;
    private bool isMoving;
    private float distance;
    private float angleToPlayer;
    private bool isAlive = true;
    private MeshRenderer weaponRenderer;
    // Start is called before the first frame update
    void Start()
    {
        HPOrg = HP;
        weaponRenderer = meleeWeapon.GetComponent<MeshRenderer>();
        //updateEnemyHPBar();

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
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
        Debug.DrawRay(headPos.position, playerDir);
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

        HP -= dmg;
        //updateEnemyHPBar();
        StartCoroutine(flashDamage());
        //UI.gameObject.SetActive(true);

        if (HP <= 0)
        {
            isAlive = false;
            agent.speed = 0;
            gameManager.instance.EnemiesInWaveCount--;
            gameManager.instance.updateTotalEnemyCount(-1);
            gameManager.instance.playerScript.coins += Random.Range(coinValueMin, coinValueMax);
            StartCoroutine(DeathAnimation());
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

    /*
    public void updateEnemyHPBar()
    {
        enemyHPBar.fillAmount = (float)HP / (float)HPOrg;
    }
    */

    /*
     * attackEntry and attackExit are set to be used with animation frames
     * The functions should be named the same on the functions tab of read-only animations i.e. Unity Asset Store Animations
     */
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
        yield return new WaitForSeconds(hitDelay);
        attackExit();
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
     * The corutines and setToFade all deal with death animations and fading out
     */
    IEnumerator DeathAnimation()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(deathAnimationTime);
        yield return StartCoroutine(DeathFadeOut());
    }

    IEnumerator DeathFadeOut()
    {
        setToFade();
        for (float t = 0.0f; t < 1.0f; t+= Time.deltaTime/deathFadeOutTime)
        {
            model.material.color = new Color(model.material.color.r, model.material.color.g, model.material.color.b, Mathf.Lerp(model.material.color.a, 0, t));
            weaponRenderer.material.color = new Color(weaponRenderer.material.color.r, weaponRenderer.material.color.g, weaponRenderer.material.color.b, Mathf.Lerp(weaponRenderer.material.color.a, 0, t));
            yield return null;
        }

        Destroy(gameObject);
    }

    void setToFade()
    {
        weaponRenderer.material.SetOverrideTag("RenderType", "Transparent");
        weaponRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        weaponRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        weaponRenderer.material.SetInt("_ZWrite", 0);
        weaponRenderer.material.DisableKeyword("_ALPHATEST_ON");
        weaponRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        weaponRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        weaponRenderer.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

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
