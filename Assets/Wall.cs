using UnityEngine;

public class Wall : MonoBehaviour {
    [Tooltip("How tall the wall is (i.e. how low it should stretch from the top of the camera).")]
    [SerializeField] float wallHeight;
    /// <summary>
    /// The Y coordinate of the bottom edge of the wall
    /// </summary>
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
