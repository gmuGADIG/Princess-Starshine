using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/**
 * Holds a single dialogue sequence or cutscene, to be played by a DialoguePlayer.
 */
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public DialogueCharacter[] characters;
    
    [TextArea(20, int.MaxValue)]
    public string text;
}

/**
 * Holds a single character for use in dialogue sequences.
 * Characters have a scriptName which is referenced in the dialogue script, a name displayed to the user, and an image.
 * To give a single character multiple variations, create multiple DialogueCharacters, e.g. Princess and PrincessSurprised
 */
[Serializable]
public class DialogueCharacter
{
    public string scriptName;
    public string displayName;
    public Texture picture;
}

/**
 * A command to be played during a dialogue sequence.
 * The name must appear exactly as it does in the script (without curly braces).
 * e.g. `{imp_leave}` will call the command with name `imp_leave`. 
 */
[Serializable]
public class DialogueCommand
{
    public string name;
    public UnityEvent action;
}