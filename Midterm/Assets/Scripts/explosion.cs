using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int pushBackAmount;
    [SerializeField] bool push;
    [SerializeField] bool effectEnemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (push)
            {
                gameManager.instance.playerScript.pushBackInput((other.transform.position - transform.position) * pushBackAmount);
            }

            else
            {
                gameManager.instance.playerScript.pushBackInput((transform.position - other.transform.position) * pushBackAmount);
            }
        }

        if (effectEnemy)
        {
            //gameManager.instance.enemyScript.pushBackInput((other.transform.position - transform.position) * pushBackAmount);
        }
    }
}
