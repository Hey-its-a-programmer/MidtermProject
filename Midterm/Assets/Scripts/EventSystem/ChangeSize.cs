using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{
    void Start()
    {
        EventManager.OpenDoorEvent += IncreaseSize;
    }

    private void IncreaseSize()
    {
        transform.localScale = new Vector3(2f, 2f, 2f);
    }

    private void OnDisable()
    {
        EventManager.OpenDoorEvent -= IncreaseSize;
    }
}
