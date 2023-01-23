using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EffectPickup : MonoBehaviour
{
    [SerializeField] StatusEffect effect;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gameManager.instance.playerScript.effectPickup(effect);
            Destroy(gameObject);
        }
    }
}
