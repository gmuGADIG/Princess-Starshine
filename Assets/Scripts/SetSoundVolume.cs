using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static OptionMenu;

public class SetSoundVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider SFX;

    public void SetSound()
    {
        mixer.SetFloat("SfxVolume", Mathf.Log10(SFX.value) * 20);
    }
}
