using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static OptionMenu;

public class SetMusicVolume : MonoBehaviour {
    public AudioMixer mixer;

    public void SetMusic(float sliderValue) {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }
}
