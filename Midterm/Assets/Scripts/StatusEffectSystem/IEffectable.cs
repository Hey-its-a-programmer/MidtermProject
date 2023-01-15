using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public void ApplyEffect(StatusEffect _data);
    public void RemoveEffect();
    public void HandleEffect();

    
}
