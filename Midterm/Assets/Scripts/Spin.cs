using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 100.0f * Time.deltaTime, 0.0f, Space.Self);
    }
}
