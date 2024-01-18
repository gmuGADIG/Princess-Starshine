using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMusic : MonoBehaviour {
    public bool isGameActive;
    public AudioSource audioSource;
    public AudioClip inGameMusic;


    // Start is called before the first frame update
    void Start() {
        isGameActive = true;
    }

    // Update is called once per frame
    void Update() {
        while (isGameActive) {
            // Set the audio clip for the AudioSource component
            audioSource.clip = inGameMusic;
            // Play the music on loop
            audioSource.loop = true;
            // Start playing the music
            audioSource.Play();
        }
    }
}