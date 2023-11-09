using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using static OptionMenu;

public class SetMusicVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider MusicVolume;
    public GameObject MusicText;

    public void SetMusic()
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolume.value) * 20);
    }

    public void Start()
    {

    }

    public void Update()
    {
        float num;
        bool volExists = mixer.GetFloat("MusicVolume", out num);
        if (volExists)
        {
            num = Mathf.Pow(10, (num / 20));
            MusicVolume.value = num;
            MusicText.GetComponent<TextMeshPro>().text = ("Music Volume: " + (num * 100) + "%");
        }
    } 
}
