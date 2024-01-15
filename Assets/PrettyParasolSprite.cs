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

    private void Update()
    {
        transform.position = Player.instance.transform.position + (Vector3)Player.instance.facingDirection * .5f;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, Player.instance.facingDirection);
    }
}
