using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreviewMenuManager : MonoBehaviour
{
    public LPNode FirstNode;
    
    public void Start() {
        LPPlayer.Instance.CurrentNode = FirstNode;
    }
}
