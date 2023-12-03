using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovementFlee : IBossMovement
{
    [SerializeField] float moveSpeed = 30f;
    [SerializeField] float fleeRadius = 5f;
    [SerializeField] Vector3[] fleePoints;

    Vector3 targetPosition;
    int currentStandPointIndex = 0;
    Transform playerTransform;
    float destinationTolerance = .1f;

    private void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        SetTargetPosition(fleePoints[currentStandPointIndex]);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > destinationTolerance)
        {
            Vector3 relativeTargetPosition = targetPosition + Camera.main.transform.position;
            relativeTargetPosition.z = 0;
            transform.position = Vector3.Lerp(transform.position, relativeTargetPosition, moveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, playerTransform.position) <= fleeRadius)
        {
            if (fleePoints.Length == 0)
            {
                Debug.LogError("No points to flee to.");
                return;
            }
            int randIndex;
            do
            {
                randIndex = Random.Range(0, fleePoints.Length);
            }
            while (randIndex == currentStandPointIndex);
            currentStandPointIndex = randIndex;
            SetTargetPosition(fleePoints[currentStandPointIndex]);
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
        targetPosition.z = 0;
    }
}
