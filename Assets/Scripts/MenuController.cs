using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public AudioSource menuMusic;

    // Load the game scene when the "Play" button is clicked.
    public void PlayGame() {
        SceneManager.LoadScene("GameScene");
    }

    // Play the in-menu music when the menu scene starts.
    private void Start() {
        menuMusic.Play();
    }
}
