using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static OptionMenu;

public class SetMusicVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider MusicVolume;

    public void SetMusic()
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolume.value) * 20);
    }

    public void Update()
    {
        //MusicVolume.value = mixer.GetFloat();
    }

}
