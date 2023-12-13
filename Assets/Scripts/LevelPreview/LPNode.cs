using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
    A spot on the map the player can be on.
*/

public class LPNode : MonoBehaviour
{
    public bool Unlocked;
    /*
    [System.NonSerialized]
    public LPNode Previous; // nullable
    public LPNode Next; // nullable
    */

    [System.NonSerialized]
    public LPNodePath PreviousPath; // nullable
    public LPNodePath NextPath; // nullable

    public string LevelName;
    public string LevelSceneName;

    IEnumerator LateStart() {
        yield return null;

        if (!Unlocked) {
            var sprite = GetComponentInChildren<SpriteRenderer>();

            if (sprite != null) {
                sprite.color = new Color(.2f, .2f, .2f);
            }
        }
    }

    public void Start() {
        if (NextPath.Enabled) {
            NextPath.StartNode = this;
            NextPath.EndNode.PreviousPath = NextPath;
            NextPath.Start();
        }
    
        StartCoroutine(LateStart());
    }
}
