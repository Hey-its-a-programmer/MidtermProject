using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shopUI : MonoBehaviour
{
    [SerializeField] private GameObject shopInterface;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            gameManager.instance.activeMenu = shopInterface;
            shopInterface.SetActive(gameManager.instance.activeMenu);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.activeMenu.SetActive(false);
            gameManager.instance.activeMenu = null;
        }
    }


}
