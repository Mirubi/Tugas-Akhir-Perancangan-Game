using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float MovementInputDiretion;

    private int amountOfJumpsLeft;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canJump;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;

    public float MovementSpeed = 10.0f;
    public float JumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float WallSlideSpeed;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection ();
        UpdateAnimation();
        CheckIfCanJump();
        CheckIfWallSliding();
    }

    private void FixedUpdate ()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding ()
    {
        if(isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else 
        {
            isWallSliding = false;
        }
    }

    private void CheckSurroundings ()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if(amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }
    private void CheckMovementDirection() 
    {
        if(isFacingRight && MovementInputDiretion < 0)
        {
            Flip();
        }
        else if (!isFacingRight && MovementInputDiretion > 0)
        {
            Flip();
        }
        if (rb.velocity.x !=0)
        {
            isWalking = true;
        }
        else 
        {
            isWalking = false;
        }
                {
        if(Mathf.Abs(rb.velocity.x) >= 0.1f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
        }
    }
    private void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        
    }
    private void CheckInput()
    {
        MovementInputDiretion = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    private void Jump ()
    {
        if(canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            amountOfJumpsLeft--;
        }
    }
    private void ApplyMovement()
    {
        rb.velocity = new Vector2(MovementSpeed * MovementInputDiretion, rb.velocity.y);

        if (isWallSliding)
        {
            if(rb.velocity.y < -WallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, - WallSlideSpeed);
            }
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

        if (wallCheck != null)
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

}

}
