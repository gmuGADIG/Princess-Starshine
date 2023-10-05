using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockOfAFeather : MonoBehaviour
{
    // Type name = value;
    [SerializeField] GameObject playerObject;
    [SerializeField] int speed;
    [SerializeField] float size;
    [SerializeField] short damage;

    private void UpdateSize() {
        float currentSize = gameObject.transform.localScale.x;
        if (currentSize != size) {
            gameObject.transform.localScale = new Vector3(size, size, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy has been hit");
        EnemyTemplate eScript = collision.gameObject.GetComponent<EnemyTemplate>();
        eScript.TakeDamage(damage);
    }

    void Update()
    {
        UpdateSize();
        gameObject.transform.RotateAround(playerObject.transform.position, Vector3.forward, speed*Time.deltaTime);
    }
}
