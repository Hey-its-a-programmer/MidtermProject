using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class audioTracks : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [SerializeField] AudioMixer Mixer;

    [SerializeField] Slider MasterVolumeSlider;
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Master"))
        {
            MasterVolumeSlider.value = PlayerPrefs.GetFloat("Master");
            MasterVolumeChange();
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            MusicVolumeSlider.value = PlayerPrefs.GetFloat("Music");
            MusicVolumeChange();
        }
        if (PlayerPrefs.HasKey("SFX"))
        {
            SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFX");
            SFXVolumeChange();
        }
    }
    public void previewSFX()
    {
        SFXSource.Play();
    }
    public void MasterVolumeChange()
    {
        PlayerPrefs.SetFloat("Master", MasterVolumeSlider.value);
        if (MasterVolumeSlider.value == 0)
            Mixer.SetFloat("Master", -80.0f);
        else
            Mixer.SetFloat("Master", Mathf.Log10(MasterVolumeSlider.value) * 20);
    }
    public void MusicVolumeChange()
    {
        PlayerPrefs.SetFloat("Music", MusicVolumeSlider.value);
        if (MusicVolumeSlider.value == 0)
            Mixer.SetFloat("Music", -80.0f);
        else
            Mixer.SetFloat("Music", Mathf.Log10(MusicVolumeSlider.value) * 20);
    }
    public void SFXVolumeChange()
    {
        PlayerPrefs.SetFloat("SFX", SFXVolumeSlider.value);
        if (SFXVolumeSlider.value == 0)
            Mixer.SetFloat("SFX", -80.0f);
        else
            Mixer.SetFloat("SFX", Mathf.Log10(SFXVolumeSlider.value) * 20);
    }
}
