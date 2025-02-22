using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Kecepatan gerakan horizontal
    public float jumpForce = 10f; // Kekuatan lompatan
    private bool isGrounded; // Apakah karakter sedang di tanah

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Mengambil input horizontal
        float moveInput = Input.GetAxis("Horizontal");

        // Mengatur kecepatan gerakan
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Mengatur animasi berdasarkan kecepatan
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Membalikkan arah karakter berdasarkan arah gerakan
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Melompat jika tombol lompat ditekan dan karakter sedang di tanah
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    // Mengecek apakah karakter menyentuh tanah
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
