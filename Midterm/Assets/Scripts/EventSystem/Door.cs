using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform movePoint;
    private bool open = false;
    public int doorID;

    private void Start()
    {
        EventManager.OpenDoorEvent += OpenDoor;

    }

    void Update() 
    {
        if(open == true)
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, 5 * Time.deltaTime);   
    }
    private void OpenDoor()
    {
        
        open = true;
    }
    private void OnDisable()
    {
        EventManager.OpenDoorEvent -= OpenDoor;
    }

}
