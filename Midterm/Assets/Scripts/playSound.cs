using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSound : MonoBehaviour
{
    [SerializeField] AudioSource soundPlayer;

    public void playThisSFX()
    {
        soundPlayer.Play();
    }
}
