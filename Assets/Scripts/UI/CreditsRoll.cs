using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsRoll : MonoBehaviour
{
    [SerializeField] Transform credits;
    [SerializeField] float scrollSpeed = 5f;

    private void Update()
    {
        credits.Translate(scrollSpeed * Time.deltaTime * Vector3.up);
    }
}
