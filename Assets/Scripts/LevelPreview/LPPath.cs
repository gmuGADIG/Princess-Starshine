using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LPPath : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] LineRenderer lineRend;

    private void Update()
    {
        if (lineRend && lineRend.positionCount == 2 && start && end)
        {
            lineRend.SetPosition(0, start.position);
            lineRend.SetPosition(1, end.position);
        }
    }

}
