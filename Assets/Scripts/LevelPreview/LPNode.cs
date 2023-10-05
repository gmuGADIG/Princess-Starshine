using System.Collections;
using System.Collections.Generic;
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

    public void Start() {
        /*
        if (Next != null) {
            Next.Previous = this;
        }
        */

        if (NextPath.Enabled) {
            NextPath.StartNode = this;
            NextPath.EndNode.PreviousPath = NextPath;
            NextPath.Start();
        }
    }
}
