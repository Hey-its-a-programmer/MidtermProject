using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(menuName = "Status Effect")]
public class StatusEffect : ScriptableObject
{
    public string Name;
    public int DamageOverTimeAmount;
    public float TickSpeed;
    [Header("----- 1 is no pentalty to movement -----")]
    [Range(1, 5)] public float MovementPentalty;
    public float Lifetime;

    public GameObject EffectParticles;
}
