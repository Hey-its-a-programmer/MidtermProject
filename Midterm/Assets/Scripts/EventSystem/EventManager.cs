using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action OpenDoorEvent;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            //if(ExampleEvent != null)
            //{
            //    ExampleEvent();
            //}// <-- This is a longer notation

            OpenDoorEvent?.Invoke(); // Shorter notation


        }
    }

    public static void StartDoorEvent()
    {
        OpenDoorEvent?.Invoke();
    }
}
