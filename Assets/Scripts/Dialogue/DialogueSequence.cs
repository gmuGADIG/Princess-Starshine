using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public DialogueCharacter[] characters;
    
    [TextArea(20, int.MaxValue)]
    public string text;
}

[Serializable]
public class DialogueCharacter
{
    public string scriptName;
    public string displayName;
    public Texture picture;
}

[Serializable]
public class DialogueEvent
{
    public string name;
    public UnityEvent action;
}