using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    


    [SerializeField] Rigidbody rb;


    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int timer;



    // Start is called before the first frame update
    void Start()
    {
        //AJ changes
        rb.velocity = (gameManager.instance.player.transform.position - transform.position) * speed;
        //
        Destroy(gameObject, timer);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
