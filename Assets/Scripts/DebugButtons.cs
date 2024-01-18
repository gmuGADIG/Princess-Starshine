using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugButtons : MonoBehaviour {
    public void OnBackButtonClick() {
        SceneManager.LoadScene("TitleScreenScene");
    }

    public void OnDialoguePreviewClick() {
        SceneManager.LoadScene("DialogueTestPreview");
    }
}
