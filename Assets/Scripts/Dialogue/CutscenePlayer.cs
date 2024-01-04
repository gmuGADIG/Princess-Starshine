using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutscenePlayer : MonoBehaviour
{
    public static CutscenePlayer instance;
    
    [HideInInspector] public Animator animator;

    void Awake()
    {
        instance = this;
        if (!TryGetComponent(out animator))
            Debug.LogError("CutscenePlayer requires an Animator component!");
    }

    public void PlayForSeconds(float seconds)
    {
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            animator.enabled = true;
            yield return new WaitForSeconds(seconds);
            animator.enabled = false;
        }
    }
}
