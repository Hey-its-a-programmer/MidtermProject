using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeWeapon : MonoBehaviour
{
    [SerializeField] int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(damage);
        }
    }
}
