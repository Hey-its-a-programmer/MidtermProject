using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform teleportDest;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.player.SetActive(false);
            gameManager.instance.player.transform.position = teleportDest.position;
            gameManager.instance.player.SetActive(true);
        }
    }
}
