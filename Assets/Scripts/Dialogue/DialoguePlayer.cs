using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    const float charsPerSecond = 20;

    [Header("Game Objects")]
    public GameObject dialogueBox;
    public TextMeshProUGUI textObj;
    public TextMeshProUGUI speakerName;
    public RawImage speakerImage;

    [Header("Dialogue Elements")]
    public DialogueSequence dialogueSequence;
    public DialogueEvent[] commands;

    string[] lines;
    int currentLineIndex = 0;
    Dictionary<string, UnityEvent> commandDict;
    Dictionary<string, DialogueCharacter> characterDict;
    
    bool isTextInProgress;
    bool skipPressed;

    public static void StartPlayer(string playerName)
    {
        var playerHolder = GameObject.Find("DialogueHolder");
        if (playerHolder == null) throw new Exception( "No dialogue holder found! Make sure all dialogue players in the scene are children of an object named `DialogueHolder`");

        var allPlayers = playerHolder.transform.Cast<Transform>().ToList();
        
        var player = allPlayers.FirstOrDefault(t => t.name == playerName);
        if (player == null) throw new Exception($"Dialogue player `{playerName}` does not exist!\nExisting players are: {string.Join(", ", allPlayers.Select(p => p.name))}");

        player.gameObject.SetActive(true);
    }


    void Start()
    {
        lines = dialogueSequence.text.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

        commandDict = new Dictionary<string, UnityEvent>();
        foreach (var command in commands)
            commandDict.Add(command.name, command.action);

        characterDict = new Dictionary<string, DialogueCharacter>();
        foreach (var character in dialogueSequence.characters)
            characterDict.Add(character.scriptName, character);
        
        ProcessLine();
    }

    void EndDialogue()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTextInProgress) skipPressed = true;
            else ProcessLine();
        }
    }

    public void HideDialogueBoxThenAdvance(float seconds)
    {
        dialogueBox.SetActive(false);
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(seconds);
            ProcessLine();
            dialogueBox.SetActive(true);
        }
    }
    
    public void ProcessLine()
    {
        if (currentLineIndex >= lines.Length)
        {
            EndDialogue();
            return;
        }
        
        var line = lines[currentLineIndex];
        var curlyIndex = line.IndexOf('{');
        var closingCurlyIndex = line.IndexOf('}');

        if (curlyIndex == -1 && closingCurlyIndex == -1)
        {
            ShowDialogue(line, null);
        }
        else if (curlyIndex != -1 && closingCurlyIndex != -1)
        {
            var dialogue = line[0 .. curlyIndex].TrimEnd();
            var command = line[(curlyIndex+1) .. closingCurlyIndex];
            if (dialogue.Length != 0) ShowDialogue(dialogue, command);
            else RunCommand(command);
        }
        else
        {
            throw new Exception($"Unbalanced curly braces in line {currentLineIndex}!\n`{line}`");
        }

        currentLineIndex += 1;
    }

    void ShowDialogue(string text, [CanBeNull] string commandOnFinish)
    {
        var colonIndex = text.IndexOf(':');
        if (colonIndex == -1) throw new Exception($"Found invalid line `{text}`! Neither a command nor valid dialogue (missing colon)");
        var speakerScriptName = text[0 .. colonIndex];
        var words = text[(colonIndex + 1) ..].TrimStart();

        if (!characterDict.ContainsKey(speakerScriptName))
        {
            throw new Exception($"Count not find character with script name `{speakerScriptName}`!");
        }
        var speakerChar = characterDict[speakerScriptName];
        speakerName.text = speakerChar.displayName;
        speakerImage.texture = speakerChar.picture;
        
        // print($"`{speaker}` is saying `{words}`");
        StartCoroutine(Coroutine());
        
        IEnumerator Coroutine()
        {
            isTextInProgress = true;
            textObj.text = "";
            var startTime = Time.time;
            for (int i = 0; i < words.Length; i++)
            {
                if (skipPressed) i = words.Length - 1; // skip to end
                else
                {
                    var elapsedTime = Time.time - startTime;
                    yield return new WaitForSeconds((i + 1) / charsPerSecond - elapsedTime);
                }
                textObj.text = words[0 .. (i+1)];

                // additional delays
                if (i == words.Length - 1) continue; // ignore for end of line
                var sentenceEnds = new[] { '.', '?', '!' };
                // if this character is a sentence-end, and the next is not
                // e.g "!!!!" will only pause on the final exclamation mark
                var isEndOfSentence = sentenceEnds.Contains(words[i]) && !sentenceEnds.Contains(words[i + 1]);
                if (isEndOfSentence)
                {
                    startTime += 0.5f;
                    yield return new WaitForSeconds(0.5f);
                }

                if (words[i] == ',')
                {
                    startTime += 0.2f;
                    yield return new WaitForSeconds(0.2f);
                }
            }

            skipPressed = false;
            isTextInProgress = false;
            if (commandOnFinish != null) RunCommand(commandOnFinish);
        }
    }

    void RunCommand(string commandName)
    {
        // print($"Running command `{commandName}`");
        if (!commandDict.ContainsKey(commandName))
        {
            Debug.LogError($"Command `{commandName}` does not exist!");
        }
        else commandDict[commandName].Invoke();
    }
}
