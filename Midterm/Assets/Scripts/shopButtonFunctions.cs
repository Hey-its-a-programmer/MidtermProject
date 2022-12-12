using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopButtonFunctions : MonoBehaviour
{



    public void upgradeDamageOnGun()
    {
        gameManager.instance.playerScript.GunList[shopUI.instance.shopSelectedGun].shootDamage += 1;

    }

    public void upgradeShootRateOnGun()
    {
        gameManager.instance.playerScript.GunList[shopUI.instance.shopSelectedGun].shootRate += 1;
    }

    public void selectNextGun()
    {
        if (shopUI.instance.shopSelectedGun < gameManager.instance.playerScript.GunList.Count - 1)
        {
            shopUI.instance.shopSelectedGun++;
 
        }

        else if (shopUI.instance.shopSelectedGun >= gameManager.instance.playerScript.GunList.Count - 1)
        {
            shopUI.instance.shopSelectedGun = 0;
        }
        Debug.Log(shopUI.instance.shopSelectedGun);
        shopUI.instance.gunShopModel = gameManager.instance.playerScript.GunList[shopUI.instance.shopSelectedGun].gunModel;
        shopUI.instance.setShopGunMeshAndMaterial();

    }

    public void selectPrevGun()
    {
        if (shopUI.instance.shopSelectedGun > 0)
        {
            shopUI.instance.shopSelectedGun--;
            
        }

        else if (shopUI.instance.shopSelectedGun <= 0)
        {
            shopUI.instance.shopSelectedGun = gameManager.instance.playerScript.GunList.Count - 1;
        }
        Debug.Log(shopUI.instance.shopSelectedGun);
        shopUI.instance.gunShopModel = gameManager.instance.playerScript.GunList[shopUI.instance.shopSelectedGun].gunModel;
        shopUI.instance.setShopGunMeshAndMaterial();

    }

}
