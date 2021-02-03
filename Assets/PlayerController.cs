using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed = 12.0F;
    private Vector2 moveDirection = Vector2.zero;
    Rigidbody2D rb;

    void Start()
    {
        // Store reference to attached component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && rb.velocity.y > -0.1 && rb.velocity.y <0.1)
        {
            rb.velocity = new Vector2(rb.velocity.x, 20);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            WorldGenerator.GenerateWorld(Random.Range(0, 10000));
        }
        float hAxis = Input.GetAxis("Horizontal");
        hAxis *= speed;
        rb.velocity = new Vector2(hAxis, rb.velocity.y);
    }
}
