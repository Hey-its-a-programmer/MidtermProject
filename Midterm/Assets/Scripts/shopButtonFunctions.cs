using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopButtonFunctions : MonoBehaviour
{
    int shopIterator = 0;
    public void selectNextGun()
    {
        if (shopIterator == 0 && shopUI.instance.gunSelection.Length == 1)
        {
            shopUI.instance.setGunModel();
        }
        else if (shopIterator < shopUI.instance.gunSelection.Length)
        {
            shopIterator++;
            shopUI.instance.setGunModel(shopIterator);
        }

    }

    public void selectPrevGun()
    {
        if (shopIterator == 0 && shopUI.instance.gunSelection.Length == 1)
        {
            shopUI.instance.setGunModel();
        }

        if (shopIterator > shopUI.instance.gunSelection.Length)
        {
            shopIterator--;
            shopUI.instance.setGunModel(shopIterator);
        }

    }

    public void buyGun()
    {
        gameManager.instance.playerScript.gunPickup(shopUI.instance.gunSelection[shopIterator]);
    }

}
