using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variabel untuk mengatur kecepatan gerakan pemain
    public float speed = 5f;

    // Variabel untuk mengatur pengurangan kecepatan saat di tanah
    [Range(0f, 1f)]
    public float groundDecay = 0.1f;

    // Variabel untuk mengatur drag (pengurangan kecepatan)
    public float drag = 0.5f;

    // Variabel untuk mengatur kekuatan lompatan
    public float jumpForce = 10f;

    // Komponen Rigidbody2D untuk fisika
    public Rigidbody2D body;

    // Komponen BoxCollider2D untuk memeriksa tanah
    public BoxCollider2D groundCheck;

    // LayerMask untuk menentukan layer tanah
    public LayerMask groundMask;

    // Status apakah pemain berada di tanah
    public bool grounded;

    // Variabel untuk menghitung jumlah lompatan
    private int jumpCount = 0;
    private const int maxJumpCount = 2; // Maksimal 2 lompatan (double jump)

    // Start is called before the first frame update
    void Start()
    {
        // Inisialisasi jika diperlukan
    }

    // Update is called once per frame
    void Update()
    {
        // Mengambil input horizontal
        float xInput = Input.GetAxis("Horizontal");

        // Menghitung arah gerakan
        Vector2 direction = new Vector2(xInput, 0).normalized;

        // Mengatur kecepatan horizontal
        body.velocity = new Vector2(direction.x * speed, body.velocity.y);

        // Melompat jika pemain berada di tanah atau sudah melakukan satu lompatan
        if (Input.GetButtonDown("Jump") && (grounded || jumpCount < maxJumpCount))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        if (grounded)
        {
            // Mengatur ulang jumlah lompatan saat berada di tanah
            jumpCount = 0;
            // Mengurangi kecepatan berdasarkan groundDecay
            body.velocity *= (1 - groundDecay);
        }
    }

    void CheckGround()
    {
        // Memeriksa apakah pemain berada di tanah
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void Jump()
    {
        // Memberikan gaya lompatan ke Rigidbody2D
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        jumpCount++; // Meningkatkan jumlah lompatan
    }
}
