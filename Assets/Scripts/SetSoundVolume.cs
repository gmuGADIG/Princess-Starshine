using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using static OptionMenu;

public class SetSoundVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider SFX;
    public GameObject SFXText;

    public void SetSound()
    {
        mixer.SetFloat("SfxVolume", Mathf.Log10(SFX.value) * 20);
    }

    public void Start()
    {

    }
    public void Update()
    {
        float num;
        bool volExists = mixer.GetFloat("SfxVolume", out num);
        if (volExists)
        {
            num = Mathf.Pow(10, (num / 20));
            SFX.value = num;
            SFXText.GetComponent<TMP_Text>().text = ("SFX Volume: " + ((int)(num * 100)) + "%");
        }
    }
}
