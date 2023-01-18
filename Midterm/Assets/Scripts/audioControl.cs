using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioControl : MonoBehaviour
{
    AudioSource mAudioSource;
    bool mPlay;
    bool mToggle;
    // Start is called before the first frame update
    void Start()
    {
        mPlay = true;
        mAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mPlay == true && mToggle == true)
        {
            mAudioSource.Play();
            mToggle = false;
        }
        if (mPlay == false && mToggle == true)
        {
            mAudioSource.Stop();
            mToggle = false;
        }
    }
    void OnGUI()
    {
        mPlay = GUI.Toggle(new Rect(10, 10, 100, 30), mPlay, "Play Music");
        if (GUI.changed)
        {
            mToggle = true;
        }
    }
}
