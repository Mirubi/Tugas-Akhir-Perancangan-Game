using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float movementInputDirection;
    private float JumpTimer;
    private float turnTimer;
    private float wallJumpTimer;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;

    public float movementSpeed = 10.0f;
    public float JumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float WallSlideSpeed;
    public float movementForceinAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallJumpDirection = new Vector2(1f, 1f).normalized;
        wallHopDirection = new Vector2(1f, 1f).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else 
        {
            isWallSliding = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }
    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            canWallJump = true;
        }
        
        if (amountOfJumpsLeft <= 0)
            {
                canNormalJump = false;
            }
            else
            {
                canNormalJump = true;
            }
    }
private void CheckMovementDirection() 
{
    if ((isFacingRight && movementInputDirection < 0) || (!isFacingRight && movementInputDirection > 0))
    {
        Flip();
    }

    isWalking = Mathf.Abs(rb.velocity.x) >= 0.1f;
}

    private void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
        
    }
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump Pressed");

            if (isGrounded || (amountOfJumps > 0 && !isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                JumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
                Debug.Log("Preparing Wall Jump");
            }
        }

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && isTouchingWall)
        {
            if (!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
            
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
            {
                checkJumpMultiplier = false;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
            }
    }
    private void CheckJump()
{
    if (JumpTimer > 0)
    {
        // WallJump
        if (!isGrounded && isTouchingWall && canWallJump)
        {
            Debug.Log("WallJump Condition Met");
            WallJump();
        }
        else if (isGrounded)
        {
            NormalJump();
        }

        if (isAttemptingToJump)
        {
            JumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }
}
    private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            amountOfJumpsLeft--;
            JumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = Vector2.zero;
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;

            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * -facingDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);

            JumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;

            Flip();
        }
    }

    private void ApplyMovement()
    {
        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }

        if (isWallSliding)
        {
            if (rb.velocity.y < -WallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -WallSlideSpeed);
            }
        }
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

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
