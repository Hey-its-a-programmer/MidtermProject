using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color newColor;

    private void Start()
    {
        EventManager.OpenDoorEvent += SetNewColor;
    }

    private void SetNewColor()
    {
        GetComponent<SpriteRenderer>().color= newColor;
    }

    private void OnDisable()
    {
        EventManager.OpenDoorEvent -= SetNewColor;
    }
}
