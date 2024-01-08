using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrettyParasolSprite : MonoBehaviour
{
    private const string swingingKey = "Swinging";
    public Animator animator;

    public Action ShouldFire;

    public bool Swinging {
        get => animator.GetBool(swingingKey);
        set => animator.SetBool(swingingKey, value);
    }

    public void OnSwingingAnimationEnd() {
        Swinging = false;
    }

    public void InvokeShouldFire() { ShouldFire?.Invoke(); }

    void Start()
    {
        PrettyParasol.Assert(TryGetComponent(out animator));
    }
}
