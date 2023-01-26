using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitPickup : MonoBehaviour
{
    [SerializeField] MedkitStats medkit;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.playerScript.HP < gameManager.instance.playerScript.HPOrig)
        {
            gameManager.instance.playerScript.HealthPickup(medkit);
            Destroy(gameObject);
        }
    }
}
