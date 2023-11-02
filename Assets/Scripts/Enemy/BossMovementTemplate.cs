using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovementTemplate : MonoBehaviour
{

    public float speed = 5f;
    public BossMovementTypes BossMovementTypes;
    private Dictionary<BossMovementTypes, IBossMovement> movementPair = new Dictionary<BossMovementTypes, IBossMovement>()
    {
        { BossMovementTypes.Wander, new BossWanderMovement() },
        //{ BossMovementTypes.Aggressive, new BossAggressiveMovement() },
        //{ BossMovementTypes.Flee, new BossFleeMovement()
    };

    public void Update()
    {
        if (movementPair.ContainsKey(BossMovementTypes))
        {
            movementPair[BossMovementTypes].Move(speed);
        }
    }

}
