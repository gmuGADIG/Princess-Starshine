using System.Collections;
using TMPro;
using UnityEngine;

/**
 * This is the Dialogue User Interface
 * By Sheldon Tran
 */
public class DialogueUI : MonoBehaviour {

    [SerializeField] private GameObject dialogueBox;
    // textName is the name text
    [SerializeField] private TMP_Text textName;
    // textLabel is the dialogue text
    [SerializeField] private TMP_Text textLabel;

    [SerializeField] private DialogueObject testDialogue;

    private TypewriterEffect typewriterEffect;

    private void Start() {
        textName.text = string.Empty;
        typewriterEffect = GetComponent<TypewriterEffect>();
        CloseDialogueBox();
        ShowDialogue(testDialogue);
    }

    public void ShowDialogue(DialogueObject dialogueObject) {
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject) {
        int counter = 0;
        foreach (string dialogue in dialogueObject.Dialogue) {
            textName.text = dialogueObject.Names[counter];
            counter++;
            yield return RunTypingEffect(dialogue);
            textLabel.text = dialogue;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        }
        CloseDialogueBox();
    }
    
    private IEnumerator RunTypingEffect(string dialogue) {
        typewriterEffect.Run(dialogue, textLabel);
        while (typewriterEffect.isRunning) {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                typewriterEffect.Stop();
            }
        }
    }

    private void CloseDialogueBox() {
        dialogueBox.SetActive(false);
        textName.text = string.Empty;
        textLabel.text = string.Empty;
    }
}
