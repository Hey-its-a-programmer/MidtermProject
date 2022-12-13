using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopButtonFunctions : MonoBehaviour
{

    private int shopSelectedGun = 0;
    private shopUI shop;
    public void upgradeDamageOnGun()
    {
        gameManager.instance.playerScript.GunList[shopSelectedGun].shootDamage += 1;

    }

    public void upgradeShootRateOnGun()
    {
        gameManager.instance.playerScript.GunList[shopSelectedGun].shootRate += 1;
    }

    public void selectNextGun()
    {

        if (shopSelectedGun == 0 && gameManager.instance.playerScript.GunList.Count - 1 == 0)
        {

        }

        else if (shopSelectedGun >= gameManager.instance.playerScript.GunList.Count - 1)
        {
            shopSelectedGun = 0;
            shop.setShopGunMeshAndMaterial(shopSelectedGun);
        }

        else if (shopSelectedGun < gameManager.instance.playerScript.GunList.Count - 1)
        {
            shopSelectedGun++;
            shop.setShopGunMeshAndMaterial(shopSelectedGun);
        }

    }

    public void selectPrevGun()
    {
        if (shopSelectedGun > 0)
        {
            shopSelectedGun--;
            shop.setShopGunMeshAndMaterial(shopSelectedGun);

        }

        else if (shopSelectedGun <= 0)
        {
            shopSelectedGun = gameManager.instance.playerScript.GunList.Count - 1;
            shop.setShopGunMeshAndMaterial(shopSelectedGun);
        }

        else
        {
            shop.setShopGunMeshAndMaterial(shopSelectedGun);
        }

    }

}
