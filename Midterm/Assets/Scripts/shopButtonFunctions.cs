using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopButtonFunctions : MonoBehaviour
{
    private int shopSelectedGun = 0;
    private List<gunStats> shopGunList;
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

        else if (shopSelectedGun >= gameManager.instance.playerScript.getGunList().Count - 1)
        {
            shopSelectedGun = 0;
        }
    }

    public void selectPrevGun()
    {
        if (shopSelectedGun > 0)
        {
            shopSelectedGun--;
        }

        else if (shopSelectedGun < 0)
        {
            shopSelectedGun = gameManager.instance.playerScript.getGunList().Count - 1;
        }
    }
}
