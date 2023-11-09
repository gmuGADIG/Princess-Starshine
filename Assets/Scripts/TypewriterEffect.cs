using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * This class implements the typewriter effect for the character dialogue
 * By Sheldon Tran
 */
public class TypewriterEffect : MonoBehaviour
{

    [SerializeField] private float typewriterSpeed = 50f;

    public bool isRunning { get; private set; }

    private Coroutine typingCoroutine;

    public void Run(string textToType, TMP_Text textLabel)
    {
        typingCoroutine = StartCoroutine(TypeText(textToType, textLabel));
    }

    public void Stop() 
    {
        StopCoroutine(typingCoroutine);
        isRunning = false;
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        isRunning = true;
        textLabel.text = string.Empty;

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * typewriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);
            yield return null;
        }

        isRunning = false;
    }
}
