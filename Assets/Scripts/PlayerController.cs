using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerController Instance;

    private float projectileFireRate = 0.5f; // Default projectile fire rate

    private Animator animator;

    private void Awake()
    {
        Instance = this;
    }

    public void SetProjectileFireRate(float newRate)
    {
        projectileFireRate = newRate;
    }
    

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for taunt input (for example, pressing the "T" key)
        if (Input.GetKeyDown(KeyCode.T))
        {
            Taunt();
        }
    }

    void Taunt()
    {
        // Trigger the taunt animation
        animator.SetTrigger("Taunt");

        // Add any additional taunt behavior here (audio, effects, etc.)
        Debug.Log("Player taunting!");
    }

}
