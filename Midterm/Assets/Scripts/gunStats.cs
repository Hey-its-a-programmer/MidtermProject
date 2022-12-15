 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public string gunName;
    public float shootRate;
    public int shootDamage;
    public int shootDist;
    public int gunPrice;
    public GameObject gunModel;
    public AudioClip gunshot;
}
   

