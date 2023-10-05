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
    public void UpdatePlayer(LPPlayer player) {
        player.transform.position = player.CurrentNode.transform.position;
    }

    public ILPState UpdateState(LPPlayer player) {
        // check for input
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (player.CurrentNode.PreviousPath != null)
                return new LPOnPath(player.CurrentNode.PreviousPath, true);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            if (player.CurrentNode.NextPath != null && player.CurrentNode.NextPath.EndNode.Unlocked)
                return new LPOnPath(player.CurrentNode.NextPath, false);

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
    }

    public void UpdatePlayer(LPPlayer player) {
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

    private ILPState state = new LPStationary();

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void Update() {
        state.UpdatePlayer(this);
        state = state.UpdateState(this);
    }
}
