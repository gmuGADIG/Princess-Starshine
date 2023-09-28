using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    A path between two `LPNode`s
*/

public interface ILPNodePath
{
    LPNode StartNode { get; set; }
    LPNode EndNode { get; set; }

    Vector3 PositionOfT(float t);
}

public class LinearNodePath : MonoBehaviour, ILPNodePath {
    public LPNode StartNode { get; set; }
    public LPNode EndNode { get; set; }
    
    public Vector3 PositionOfT(float t) {
        return Vector3.Lerp(StartNode.transform.position, EndNode.transform.position, t);
    }
}