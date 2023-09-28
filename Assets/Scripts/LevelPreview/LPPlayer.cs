using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    The player traversing the LevelPreviewMenu
*/

public class LPPlayer : MonoBehaviour
{
    public static LPPlayer Instance;
    [System.NonSerialized]
    public LPNode CurrentNode;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void Update() {
        // check for input
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (CurrentNode.Previous != null)
                CurrentNode = CurrentNode.Previous;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            if (CurrentNode.Next != null && CurrentNode.Next.Unlocked)
                CurrentNode = CurrentNode.Next;
        // set position to CurrentNode's position
        transform.position = CurrentNode.transform.position;
    }
}
