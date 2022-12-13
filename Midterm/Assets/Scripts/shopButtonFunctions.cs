using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopButtonFunctions : MonoBehaviour
{
    public void selectNextGun()
    {
        shopUI.instance.next();
    }

    public void selectPrevGun()
    {
        shopUI.instance.prev();
    }

    public void buyGun()
    {
        shopUI.instance.buy();
    }

}
