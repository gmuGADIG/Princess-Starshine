using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static OptionMenu;

public class SetSoundVolume : MonoBehaviour {
    public AudioMixer mixer;

    public void SetSound(float sliderValue) {
        mixer.SetFloat("SFXSoundVolume", Mathf.Log10(sliderValue) * 20);
    }
}
