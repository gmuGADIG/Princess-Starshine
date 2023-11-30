using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ConstantMove : MonoBehaviour
{
    public float velocity = 1000f;
    public float life = 1000f;
    Rigidbody2D m_Rigidbody;
    SpriteRenderer m_SpriteRenderer;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer.enabled = true;
        target = GameObject.Find("Circle").transform;
        Vector2 targetDir = target.position - transform.position;
        float angle = Vector2.Angle(targetDir, transform.forward);
        Debug.Log(angle);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetDir = target.position - transform.position;
        float angle = Vector2.Angle(targetDir, transform.forward);
        m_Rigidbody.velocity = new Vector2(velocity * Time.deltaTime, 0);
        life -= 1;
        if(life < 0)
        {
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
