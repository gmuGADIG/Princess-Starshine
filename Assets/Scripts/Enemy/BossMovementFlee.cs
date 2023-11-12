using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovementFlee : MonoBehaviour
{
    [SerializeField] float moveSpeed = 30f;
    [SerializeField] float fleeRadius = 5f;
    [SerializeField] Vector3[] standPoints;

    Vector3 targetPosition;
    int currentStandPointIndex = 0;
    Transform playerTransform;
    float destinationTolerance = .1f;

    private void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        SetTargetPosition(standPoints[currentStandPointIndex]);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > destinationTolerance)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) <= fleeRadius)
        {
            int randIndex;
            do
            {
                randIndex = Random.Range(0, standPoints.Length);
            }
            while (randIndex == currentStandPointIndex);
            currentStandPointIndex = randIndex;
            SetTargetPosition(standPoints[currentStandPointIndex]);
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position + Camera.main.transform.position;
        targetPosition.z = 0;
    }
}
