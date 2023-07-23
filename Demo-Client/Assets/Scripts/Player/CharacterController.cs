using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int moveSpeed = 10;
    public int jumpSpeed = 15;
    private Rigidbody2D r2d;
    private bool isLand = true;
    private void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

        Move();
        Jump();
    }
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            r2d.velocity = new Vector2(h * moveSpeed, r2d.velocity.y);
        }
    }
    private void Jump()
    {
        if (isLand && Input.GetKeyDown(KeyCode.W))
        {
            r2d.velocity = new Vector2(r2d.velocity.x, jumpSpeed);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        isLand = true;
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        isLand = false;
    }
}
