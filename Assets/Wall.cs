using UnityEngine;

public class Wall : MonoBehaviour {
    [SerializeField] float wallHeight;
    public float Border { get => TeaTime.cameraBoundingBox().yMax - wallHeight; }

    void OnDrawGizmos() {
        var rect = TeaTime.cameraBoundingBox();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector2(rect.xMin, rect.yMax),
            new Vector2(rect.xMax, rect.yMax)
        );

        Gizmos.DrawLine(
            new Vector2(rect.xMin, rect.yMax - wallHeight),
            new Vector2(rect.xMax, rect.yMax - wallHeight)
        );

    }
}
