using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class shopUI : MonoBehaviour
{
    [SerializeField] private GameObject shopInterface;
    [SerializeField] TextMeshProUGUI shopGunSelected;
    [SerializeField] private GameObject pressE;
    private bool triggerActive = false;

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            triggerActive = true;
            pressE.SetActive(true);
        }
        
    }

    private void Update()
    {
        if (triggerActive && Input.GetKey(KeyCode.E))
        {
            shopActions();
            pressE.SetActive(false);
        }

        if (Input.GetButtonDown("Cancel") && gameManager.instance.activeMenu == shopInterface)
        {
            gameManager.instance.activeMenu.SetActive(false);
            gameManager.instance.activeMenu = null;
            pressE.SetActive(true);
        }
    }

    private void shopActions()
    {
        gameManager.instance.activeMenu = shopInterface;
        gameManager.instance.activeMenu.SetActive(true);
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager.instance.activeMenu != null)
            {
                gameManager.instance.activeMenu.SetActive(false);
                gameManager.instance.activeMenu = null;
            }

            pressE.SetActive(false);
        }
    }


}
