using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7f; // Kecepatan gerakan horizontal
    public float jumpForce = 12f; // Kekuatan lompatan
    private bool isGrounded; // Apakah karakter sedang di tanah
    private int jumpCount = 0; // Menghitung jumlah lompatan
    private int maxJumps = 2; // Jumlah maksimum lompatan

    public float attackCooldown = 0.5f; // Waktu cooldown antara serangan
    private float nextAttackTime = 0f; // Waktu berikutnya serangan dapat dilakukan

    public Rigidbody2D rb;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Mengambil input horizontal
        float moveInput = Input.GetAxis("Horizontal");

        // Mengatur kecepatan gerakan langsung
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Mengatur animasi berdasarkan kecepatan
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Membalikkan arah karakter berdasarkan arah gerakan
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        // Melompat jika tombol lompat ditekan dan jumlah lompatan belum mencapai maksimum
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
            animator.SetBool("isJumping", true);
        }

        // Serang jika tombol serangan ditekan dan waktu cooldown telah habis
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    // Mengecek apakah karakter menyentuh tanah
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; // Reset jumlah lompatan saat menyentuh tanah
            OnLanding(); // Memicu event OnLanding
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Metode untuk menangani serangan
    private void Attack()
    {
        // Memainkan animasi serangan
        animator.SetTrigger("Attack");

        // Logika serangan (misalnya, mengirimkan proyektil atau mengeksekusi efek serangan)
        // Contoh: Instantiate(attackProjectile, attackPoint.position, transform.rotation);
    }
}
