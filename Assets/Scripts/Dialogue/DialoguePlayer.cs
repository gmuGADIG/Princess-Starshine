using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * An object which plays a single dialogue sequence as soon as it's enabled.
 * To trigger a sequence at the start of a scene, have enable it in the inspector.
 * To trigger it on demand, see DialoguePlayer.StartPlayer.
 */
public class DialoguePlayer : MonoBehaviour
{
    const float charsPerSecond = 100;

    [Header("Game Objects")]
    public GameObject dialogueBox;
    public TextMeshProUGUI textObj;
    public TextMeshProUGUI speakerName;
    public RawImage speakerImage;

    [Header("Dialogue Elements")]
    public DialogueSequence dialogueSequence;
    public DialogueCommand[] commands;

    [Header("Events")]
    [Tooltip("Fired when the dialogue begins.")]
    public UnityEvent startEvent;
    [Tooltip("When the player skips the dialogue, this event should set everything to its final state.\n" +
             "e.g. if the princess walks around, this should set her position to where she stops.\n" +
             "note that endEvent is also invoked when the dialogue is skipped.")]
    public UnityEvent onSkip;
    
    [Tooltip("After the dialogue ends, this object is disabled and this event is invoked.")]
    public UnityEvent endEvent;

    [Tooltip("After the player has read through past three dialogue options, this event is invoked.")]
    public UnityEvent ReadThree;

    string[] lines;
    int currentLineIndex = 0;
    Dictionary<string, UnityEvent> commandDict;
    static Dictionary<string, DialogueCharacter> characterDict;
    
    bool isTextInProgress;

    public static DialoguePlayer currentPlayer;

    /**
     * Starts a dialogue player based on the name of the game object it's attached to.
     * For this to work, make sure all dialogue players are under an enabled game object named DialogueHolder. 
     */
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
        ReadThree.AddListener(() => print("ReadThree.Invoke"));

        currentPlayer = this;
        
        startEvent.Invoke();
        lines = dialogueSequence.text.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

        SetUpCommandAndCharacterDict();
        ProcessLine();
    }

    void OnDisable()
    {
        print($"dialogue {this.name} disabled");
    }

    void SetUpCommandAndCharacterDict()
    {
        if (commandDict == null)
        {
            commandDict = new Dictionary<string, UnityEvent>();
            foreach (var command in commands)
                commandDict.Add(command.name, command.action);
        }
        
        if (characterDict == null)
        {
            characterDict = new Dictionary<string, DialogueCharacter>();
            var allCharacters = Resources.LoadAll<DialogueCharacter>("Dialogue/Characters");
            if (allCharacters.Length == 0) Debug.LogError("No character resources were loaded! Dialogue will not work");
            foreach (var character in allCharacters)
                characterDict.Add(character.scriptName, character);
        }
    }

    int numRead;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // advance
        {
            if (dialogueBox.activeSelf == false) return;
            if (isTextInProgress) return;
            else {
                numRead += 1;
                if (numRead >= 3) { 
                    // animation shouldn't invoke more than nessasary, since 
                    // the animation disables the game object
                    ReadThree.Invoke(); 
                }

                ProcessLine();
            }
        }

        if (Input.GetKeyDown(KeyCode.BackQuote)) SkipDialogue();
    }

    void SkipDialogue()
    {
        this.gameObject.SetActive(false);
        onSkip.Invoke();
        endEvent.Invoke();
    }

    /**
     * Runs a single line of the dialogue sequence, displaying the text and triggering any commands.
     * Advances the currentLineIndex by one.
     * If there are no more lines to read, ends the dialogue and hides the box. 
     */
    public void ProcessLine()
    {
        int thisLineIndex = currentLineIndex;
        currentLineIndex += 1;
        
        if (thisLineIndex >= lines.Length)
        {
            this.gameObject.SetActive(false);
            endEvent.Invoke();
            return;
        }

        var line = lines[thisLineIndex];
        var curlyIndex = line.IndexOf('{');
        var closingCurlyIndex = line.IndexOf('}');

        var doublePercentIndex = line.IndexOf("%%");
        if (doublePercentIndex != -1)
        {
            var voiceLine = line[(doublePercentIndex + 2) ..];
            print($"playing dialogue voice line {voiceLine}");
            SoundManager.Instance.PlaySoundGlobal(voiceLine);
        }

        if (curlyIndex == -1 && closingCurlyIndex == -1)
        {
            if (doublePercentIndex == -1)
                ShowDialogue(line, null);
            else
                ShowDialogue(line[0 .. doublePercentIndex], null);
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
            throw new Exception($"Unbalanced curly braces in line {thisLineIndex}!\n`{line}`");
        }
    }

    /**
     * Parses a string in the form `character: text to display` (no commands!) and displays it.
     */
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
        dialogueBox.SetActive(true);
        
        // make character bob
        foreach (var bobber in FindObjectsOfType<DialogueBobber>())
            bobber.CheckBob(speakerScriptName);
        
        StartCoroutine(Coroutine());
        
        IEnumerator Coroutine()
        {
            isTextInProgress = true;
            textObj.text = "";
            var startTime = Time.time;
            for (int i = 0; i < words.Length; i++)
            {
                var elapsedTime = Time.time - startTime;
                yield return new WaitForSeconds((i + 1) / charsPerSecond - elapsedTime);
                
                textObj.text = words[0 .. (i+1)];
            }

            isTextInProgress = false;
            if (commandOnFinish != null) RunCommand(commandOnFinish);
        }
    }

    /**
     * Runs a command based on its name (commandName must not contain curly braces).
     */ 
    void RunCommand(string commandName)
    {
        // print($"Running command `{commandName}`");
        if (!commandDict.ContainsKey(commandName))
        {
            Debug.LogError($"Command `{commandName}` does not exist!");
        }
        else commandDict[commandName].Invoke();
    }

    #region validation
    public void ValidateDialogue()
    {
        // initial validation
        if (transform.parent.name != "DialogueHolder") Debug.LogError("DialoguePlayer must be child to an object named \"DialogueHolder\"");
        
        if (dialogueSequence == null)
        {
            Debug.LogError("Dialogue player is missing a DialogueSequence!");
            return;
        }
        
        // set up problem lists
        var missingCharacters = new HashSet<string>();
        var missingCommands = new HashSet<string>();
        var missingVoiceLines = new HashSet<string>();

        // read characters (from resources) and commands (from the serialized command list)
        characterDict = null;
        commandDict = null;
        SetUpCommandAndCharacterDict();
        
        // read voice sound resources
        var soundResourceSet = new HashSet<String>();
        foreach (var soundResource in Resources.LoadAll<Sound>("Sounds/"))
            soundResourceSet.Add(soundResource.name);

        // process all lines and verify them
        var allLines = dialogueSequence.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in allLines)
        {
            var curlyIndex = line.IndexOf('{');
            var closingCurlyIndex = line.IndexOf('}');

            var doublePercentIndex = line.IndexOf("%%");
            if (doublePercentIndex != -1)
            {
                var voiceLine = line[(doublePercentIndex + 2) ..];
                VerifyVoiceLine(voiceLine);
            }
            
            if (curlyIndex == -1 && closingCurlyIndex == -1)
            {
                if (doublePercentIndex == -1)
                    VerifyDialogue(line);
                else
                    VerifyDialogue(line[0 .. doublePercentIndex]);
            }
            else if (curlyIndex != -1 && closingCurlyIndex != -1)
            {
                var dialogue = line[0 .. curlyIndex].TrimEnd();
                var command = line[(curlyIndex+1) .. closingCurlyIndex];
                if (dialogue.Length != 0) VerifyDialogue(dialogue);
                VerifyCommand(command);
            }
            else
            {
                Debug.LogError($"Invalid line `{line}`!\n(Unbalanced curly braces)");
            }
        }

        if (missingCharacters.Count == 0) Debug.Log("All characters are accounted for.");
        else Debug.LogError($"Found {missingCharacters.Count} missing characters! {string.Join(", ", missingCharacters)}");
        
        if (missingCommands.Count == 0) Debug.Log("All commands are accounted for.");
        else Debug.LogError($"Found {missingCommands.Count} missing commands! {string.Join(", ", missingCommands)}");
        
        if (missingVoiceLines.Count == 0) Debug.Log("All voice lines are accounted for.");
        else Debug.LogError($"Found {missingVoiceLines.Count} missing voice lines! {string.Join(", ", missingVoiceLines)}");
        
        Debug.Log("Validation finished.");
        
        void VerifyDialogue(string line)
        {
            var colonIndex = line.IndexOf(':');
            if (colonIndex == -1) throw new Exception($"Invalid line `{line}`!\nLine is neither a valid command nor dialogue (missing colon)");
            var speakerScriptName = line[0 .. colonIndex];
            if (characterDict.ContainsKey(speakerScriptName) == false) missingCharacters.Add(speakerScriptName);
        }

        void VerifyCommand(string command)
        {
            if (commandDict.ContainsKey(command) == false) missingCommands.Add(command);
        }

        void VerifyVoiceLine(string soundName)
        {
            if (soundResourceSet.Contains(soundName) == false) missingVoiceLines.Add(soundName);
        }
    }
    #endregion
    
    #region UnityEvent helper functions
    /**
     * Helpful function for commands. If you want to play an animation, use this to hide the dialogue box until it finishes.
     */
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

    /**
     * Simply calls SceneManager.LoadScene(scene). Necessary to change scene in UnityEvents.
     */
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    /**
     * Changes scenes to the level select and unlocks the next level.
     */
    public void OpenLevelPreview(string nextLevel)
    {
        SaveManager.SaveData.FurthestLevelSceneName = nextLevel;
        SceneManager.LoadScene("LevelPreview");
    }

    /**
     * Destroys all enemies (objects with an EnemyTemplate component), without dropping xp.
     */
    public void DeleteEnemies()
    {
        foreach (var enemy in FindObjectsOfType<EnemyTemplate>())
            Destroy(enemy.gameObject);
    }

    /**
     * Finds and activates the EnemySpawner, BossWeapon, and IBossMovement component in the current scene.
     */
    public void ActivateBossAndEnemies()
    {
        FindObjectOfType<EnemySpawner>().enabled = true;
        FindObjectOfType<BossWeapon>().enabled = true;
        FindObjectOfType<IBossMovement>().enabled = true;
    }

    /**
     * Hides the dialogue box while the given GameObject walks towards the object with tag DeadBoss
     */
    public void HideDialogueAndWalkToDeadBoss(Transform walker)
    {
        var boss = GameObject.FindGameObjectWithTag("DeadBoss");
        if (boss == null) throw new Exception("Couldn't find dead boss!");
        
        HideDialogueAndWalkTo(walker, boss.transform.position, 1.5f);
    }

    public void HideDialogueAndWalkDeadBossToObject(Transform destination)
    {
        var boss = GameObject.FindGameObjectWithTag("DeadBoss");
        if (boss == null) throw new Exception("Couldn't find dead boss!");

        HideDialogueAndWalkTo(boss.transform, destination.position);
    }

    public void ExplodeHouse()
    {
        var boss = GameObject.FindGameObjectWithTag("DeadBoss");
        if (boss == null) throw new Exception("Couldn't find dead boss!");

        Instantiate(Resources.Load<GameObject>("HouseExplosionParticles"), boss.transform.position, Quaternion.identity);
        Destroy(boss.gameObject);
    }

    /**
     * Hides the dialogue box while the given object walks towards the destination, stopping when it's `tolerance` meters away.
     */
    public void HideDialogueAndWalkTo(Transform walker, Vector3 destination, float tolerance = 0.05f)
    {
        StartCoroutine(Coroutine());
        
        IEnumerator Coroutine()
        {
            dialogueBox.SetActive(false);
            while (true)
            {
                walker.position =
                    Vector3.MoveTowards(
                        walker.position, 
                        destination,
                        5 * Time.deltaTime
                    );
                
                var distance = (walker.position - destination).magnitude;
                if (distance < tolerance) break;
                else yield return new WaitForEndOfFrame();
            }
            dialogueBox.SetActive(true);
            ProcessLine();
        }
    }

    public void DestroyDeadBoss()
    {
        GameObject.FindGameObjectWithTag("DeadBoss").gameObject.SetActive(false);
    }

    public void SetPlayerActive(bool active)
    {
        Player.instance.enabled = active;
        EquipmentManager.instance.gameObject.SetActive(active);
        if (CutscenePlayer.instance != null) CutscenePlayer.instance.animator.enabled = false;
    }

    #endregion
}

