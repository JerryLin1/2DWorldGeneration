using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    float speed = 12.0F;
    private Vector2 moveDirection = Vector2.zero;
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    Grid grid;
    TileMapCreator tRenderer;
    Tilemap tilemapSolid;
    Camera cam;
    Animator animator;
    bool grounded = true;

    void Start()
    {
        // Store reference to attached component
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        tRenderer = grid.transform.GetComponent<TileMapCreator>();
        tilemapSolid = grid.transform.GetChild(0).GetComponent<Tilemap>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Raycast down to see if player is on ground
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position - new Vector3(boxCollider2D.size.x / 2, 0), -Vector2.up, boxCollider2D.size.y / 2 + 0.2f);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(boxCollider2D.size.x / 2, 0), -Vector2.up, boxCollider2D.size.y / 2 + 0.2f);
        Debug.DrawRay(transform.position - new Vector3(boxCollider2D.size.x / 2, 0), -Vector2.up);
        Debug.DrawRay(transform.position + new Vector3(boxCollider2D.size.x / 2, 0), -Vector2.up);
        if (hitLeft.collider == null && hitRight.collider == null) SetGrounded(false);
        else SetGrounded(true);
        if (grounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.velocity = new Vector2(rb.velocity.x, 20);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            WorldGenerator.GenerateWorld(Random.Range(0, 10000));
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseTilePos = grid.WorldToCell(mousePos);
            if (WorldGenerator.IsValidCell(mouseTilePos.x, mouseTilePos.y) && WorldGenerator.GetCellFg(mouseTilePos.x, mouseTilePos.y) != 0)
            {
                WorldGenerator.DestroyTileFg(mouseTilePos.x, mouseTilePos.y);
                tRenderer.UpdateTileFg(mouseTilePos.x, mouseTilePos.y);
            }
        }
        float hAxis = Input.GetAxis("Horizontal");
        if (Mathf.Abs(hAxis) > 0.1)
        {
            if (hAxis < 0) transform.localRotation = Quaternion.Euler(0, 180, 0);
            else transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (grounded) animator.SetBool("isMoving", true);
            else animator.SetBool("isMoving", false);
        }
        else animator.SetBool("isMoving", false);
        hAxis *= speed;
        rb.velocity = new Vector2(hAxis, rb.velocity.y);
    }
    void SetGrounded(bool isGrounded)
    {
        grounded = isGrounded;
        animator.SetBool("grounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }
}
