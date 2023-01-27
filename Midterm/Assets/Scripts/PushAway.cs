using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAway : MonoBehaviour
{
    [SerializeField] int pushForce;
    private Vector3 pushDir;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.PushAway((other.transform.position - transform.position) * pushForce);
        }
    }
}
