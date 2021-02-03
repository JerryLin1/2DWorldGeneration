using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed = 120.0F;
    private Vector2 moveDirection = Vector2.zero;
    Rigidbody2D rb;

    void Start()
    {
        // Store reference to attached component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        rb.velocity = moveDirection;
    }
}
