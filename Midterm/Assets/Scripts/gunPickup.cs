using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gun;
    public TextMeshProUGUI ammoInfoText;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.gunPickup(gun);
            Destroy(gameObject);
        }
    }






}
