using UnityEngine;

/**
 * Holds a single character for use in dialogue sequences.
 * Characters have a scriptName which is referenced in the dialogue script, a name displayed to the user, and an image.
 * To give a single character multiple variations, create multiple DialogueCharacters, e.g. Princess and PrincessSurprised
 */
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueCharacter")]
public class DialogueCharacter : ScriptableObject {
    public string scriptName;
    public string displayName;
    public Texture picture;
}