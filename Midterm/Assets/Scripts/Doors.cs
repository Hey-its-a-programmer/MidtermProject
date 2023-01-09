using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public GameObject teleportDest;
    public GameObject player;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            player.transform.position = teleportDest.transform.position;
        }
    }
}
