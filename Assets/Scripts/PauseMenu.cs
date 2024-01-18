using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] string mainMenuSceneName = "TitleScreenScene";

    private void Awake() {
        pauseMenu.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // If the pause menu is closed and nothing else is pausing the game:
            if (!pauseMenu.activeSelf && Time.timeScale != 0f) {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
            } else if (pauseMenu.activeSelf) { // if the pause menu is open:
                Resume();
            }
        }
    }

    public void Resume() {
        if (optionsMenu.activeSelf) { return; }
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart() {
        if (optionsMenu.activeSelf) { return; }
        Time.timeScale = 1f;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReturnToMainMenu() {
        if (optionsMenu.activeSelf) { return; }
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OpenOptionsMenu() {
        optionsMenu.SetActive(true);
    }

    public void ExitGame() {
        if (optionsMenu.activeSelf) { return; }
        Time.timeScale = 1f;
        Application.Quit();
    }
}
