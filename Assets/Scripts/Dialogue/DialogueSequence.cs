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
    [TextArea(20, int.MaxValue)]
    public string text;
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