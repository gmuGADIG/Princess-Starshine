using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    A path between two `LPNode`s
*/

/*
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
*/

[System.Serializable]
public class LPNodePath {
    public bool Enabled { get { return EndNode != null; } }

    public GameObject[] Stops;

    [System.NonSerialized]
    public LPNode StartNode;
    public LPNode EndNode;

    public float Length { get; private set; } = 0;

    private (float, Vector3)[] lut;

    public void Start() {
        lut = new (float, Vector3)[Stops.Length + 2];
        int cursor = 0;

        lut[0] = (0.0f, StartNode.transform.position);
        for (int idx = 1; idx < Stops.Length + 1; idx++) { // idx indexs into lut
            (_, Vector3 p1) = lut[idx - 1];
            Vector3 p2 = Stops[idx - 1].transform.position;
            Length += (p2 - p1).magnitude;
            lut[idx] = (Length, p2);
        }
        
        {
            (_, Vector3 p1) = lut[lut.Length - 2];
            Vector3 p2 = EndNode.transform.position;
            Length += (p2 - p1).magnitude;

            lut[lut.Length - 1] = (Length, p2);
        }
    }

    public Vector3 PositionOfDistance(float distance) {
        // handle a couple of edge cases
        if (distance <= 0) {
            (_, Vector3 pos) = lut[0];
            return pos;
        }

        if (distance >= Length) {
            (_, Vector3 pos) = lut[lut.Length - 1];
            return pos;
        }
        
        for (int idx = 0; idx < lut.Length; idx++) {
            // find the segment, specified by distance
            (float d, Vector3 pos) = lut[idx];

            if (d >= distance) { // we have found the first stop in front of distance
                (float prevD, Vector3 prevPos) = lut[idx - 1]; // and we can find the stop behind distance

                // determine how deep we are in the segment as a percentage
                float lengthOfSegment = d - prevD;
                float depthInSegment = distance - prevD;
                float percentInSegment = depthInSegment / lengthOfSegment;

                // then use that percentage to find a vector describing where the player should be
                Vector3 segmentVector = (pos - prevPos);
                return prevPos + (segmentVector * percentInSegment);
            }
        }

        Debug.LogError("Unreachable");
        return new Vector3();
    }
}