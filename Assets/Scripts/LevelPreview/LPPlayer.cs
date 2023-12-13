using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    The player traversing the LevelPreviewMenu
*/

interface ILPState {
    void UpdatePlayer(LPPlayer player);
    ILPState UpdateState(LPPlayer player);
}

class LPStationary : ILPState {
    public LPStationary() {
        LPPlayer.FireAtLevel(LPPlayer.Instance.CurrentNode.LevelName);
    }

    public void UpdatePlayer(LPPlayer player) {
        player.transform.position = player.CurrentNode.transform.position;
    }

    public ILPState UpdateState(LPPlayer player) {
        if (LevelPreviewMenuManager.OptionsMenuVisible) return this;

        // check for input
        if (Input.GetKeyDown(KeyCode.A))
            if (player.CurrentNode.PreviousPath != null)
                return new LPOnPath(player.CurrentNode.PreviousPath, true);

        if (Input.GetKeyDown(KeyCode.D))
            if (player.CurrentNode.NextPath != null && player.CurrentNode.NextPath.EndNode.Unlocked)
                return new LPOnPath(player.CurrentNode.NextPath, false);
        
        if (Input.GetKeyDown(KeyCode.Space))
            if (player.CurrentNode.LevelSceneName != "")
                LPPlayer.FireSwapToLevel(player.CurrentNode.LevelSceneName);

        return this;
    }
}

class LPOnPath : ILPState {
    private LPNodePath path;
    private bool reverse;

    private float distanceTraveled = 0;

    public LPOnPath(LPNodePath path, bool reverse) { 
        this.path = path; 
        this.reverse = reverse;
        LPPlayer.FireLeftLevel();
    }

    public void UpdatePlayer(LPPlayer player) {
        if (LevelPreviewMenuManager.OptionsMenuVisible) return;
        
        distanceTraveled = Mathf.Min(
            distanceTraveled + (player.Speed * Time.deltaTime),
            path.Length
        );

        float distance = reverse ? path.Length - distanceTraveled : distanceTraveled;

        player.transform.position = path.PositionOfDistance(distance);
    }

    public ILPState UpdateState(LPPlayer player) {
        if (distanceTraveled >= path.Length) {
            if (reverse) {
                player.CurrentNode = path.StartNode;
            } else {
                player.CurrentNode = path.EndNode;
            }

            return new LPStationary();
        } return this;
    }
}

public class LPPlayer : MonoBehaviour
{
    public float Speed = 15; // units per second

    public static LPPlayer Instance;
    [System.NonSerialized]
    public LPNode CurrentNode;

    private ILPState state;

    public static event Action<string> AtLevel;
    public static event Action LeftLevel;
    public static event Action<string> SwapToLevel;

    public static void FireAtLevel(string levelName) { AtLevel(levelName); }
    public static void FireLeftLevel() { LeftLevel(); }
    public static void FireSwapToLevel(string sceneName) { SwapToLevel(sceneName); }

    public void Awake() {
        Instance = this;
    }

    public void Start() {
        state = new LPStationary();
    }

    public void Update() {
        state.UpdatePlayer(this);
        state = state.UpdateState(this);
    }
}
