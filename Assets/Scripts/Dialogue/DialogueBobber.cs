using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * This script allows objects to bob up a bit whenever they talk in a cutscene, indicating who's talking and adding a bit more movement. 
 */
public class DialogueBobber : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of all DialogueCharacter script names that this object is associated with. If any of these are talking, the object will bob.")]
    string[] scriptNames;

    /**
     * If the script name matches, bob the character up an down.
     */
    public void CheckBob(string scriptName)
    {
        if (scriptNames.Contains(scriptName)) Bob();
    }

    void Bob()
    {
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            const float height = 0.2f, length = 0.18f;
            
            var elapsed = 0f;
            var previousHeight = 0f;
            while (elapsed < length)
            {
                // over-engineered parabola to make it modulated by the height and duration
                var expectedHeight = (4 * height * elapsed * (length - elapsed)) / (length * length);
                var heightDiff = expectedHeight - previousHeight;
                this.transform.position += Vector3.up * heightDiff;
                previousHeight = expectedHeight;
                
                elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
