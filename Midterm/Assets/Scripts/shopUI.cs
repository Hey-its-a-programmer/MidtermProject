using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shopUI : MonoBehaviour
{
    [SerializeField] private GameObject shopInterface;
    private bool triggerActive = false;
    private int shopSelectedGun;
    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Player"))
        {
            triggerActive = true;
        }
        
    }

    private void Update()
    {
        if (triggerActive && Input.GetKeyDown("Enter"))
        {
            shopActions();
        }
 
    }

    private void shopActions()
    {

        gameManager.instance.activeMenu = shopInterface;
        shopInterface.SetActive(gameManager.instance.activeMenu);
    }

    public void upgradeDamageOnGun()
    {
        gameManager.instance.playerScript.getGunList()[shopSelectedGun].shootDamage += 1;
    }

    public void upgradeShootRateOnGun()
    {
        gameManager.instance.playerScript.getGunList()[shopSelectedGun].shootRate += 1;
    }

    public void selectNextGun()
    {
        if (shopSelectedGun < gameManager.instance.playerScript.getGunList().Count - 1)
        {
            shopSelectedGun++;
        }
    }

    public void selectPrevGun()
    {
        if (shopSelectedGun > 0)
        {
            shopSelectedGun--;
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
