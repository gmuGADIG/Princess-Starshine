using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    const string musicVolKey = "musicVolume";
    const string sfxVolKey = "sfxVolume";

    public UnityEvent OnClose;
    public AudioMixer mixerGroup;

    public Slider musicSlider;
    public Slider sfxSlider;

    void InitalizeVolume() {
        print($"musicSlider: {musicSlider == null}");
        musicSlider.value = PlayerPrefs.GetFloat(musicVolKey, 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxVolKey, 0.75f);

        UpdateVolume();
    }

    public void Start() {
        InitalizeVolume();
        gameObject.SetActive(false);
    }

    public void Close() {
        gameObject.SetActive(false);
        OnClose.Invoke();
    }

    public void UpdateVolume() {
        mixerGroup.SetFloat(musicVolKey, Mathf.Log10(musicSlider.value) * 20);
        mixerGroup.SetFloat(sfxVolKey, Mathf.Log10(sfxSlider.value) * 20);

        PlayerPrefs.SetFloat(musicVolKey, musicSlider.value);
        PlayerPrefs.SetFloat(sfxVolKey, sfxSlider.value);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Close();
        }

        UpdateVolume();
    }

    void OnDestroy() {
        Debug.Log(PlayerPrefs.GetFloat(musicVolKey, 0.75f));
        Debug.Log(PlayerPrefs.GetFloat(sfxVolKey, 0.75f));
    }
}
