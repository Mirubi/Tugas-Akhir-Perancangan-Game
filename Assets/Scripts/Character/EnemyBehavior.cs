using System.Collections;
using UnityEngine;

public class EnemyBehavior : CharacterStats
{
    private enum State
    {
        Patrolling,
        Chasing,
        Attacking,
        Returning
    }

    [Header("Enemy Specific")]
    public int attackDamage = 1;
    public Transform[] patrolPoints;
    public float speed = 2f;
    private int destPoint = 0;

    public float detectionRange = 5f;
    public float attackRange = 1f;
    public LayerMask playerLayer;
    private Transform player;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;
    public float attackDamageDelay = 0.5f;

    private State currentState;

    public override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentState = State.Patrolling;

        if (patrolPoints.Length > 0)
        {
            FlipTowards(patrolPoints[destPoint].position);
        }
    }

    void Update()
    {
        if (currentHealth <= 0 || player == null) return;

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                if (IsPlayerInDetectionRange())
                {
                    currentState = State.Chasing;
                }
                break;
            case State.Chasing:
                ChasePlayer();
                if (IsPlayerInAttackRange())
                {
                    currentState = State.Attacking;
                }
                else if (!IsPlayerInDetectionRange())
                {
                    currentState = State.Returning;
                }
                break;
            case State.Attacking:
                Attack();
                if (!IsPlayerInAttackRange())
                {
                    currentState = State.Chasing;
                }
                break;
            case State.Returning:
                ReturnToPatrolPath();
                if (IsPlayerInDetectionRange())
                {
                    currentState = State.Chasing;
                }
                break;
        }
    }

    private bool IsPlayerInDetectionRange()
    {
        return Vector2.Distance(transform.position, player.position) < detectionRange;
    }

    private bool IsPlayerInAttackRange()
    {
        return Vector2.Distance(transform.position, player.position) <= attackRange;
    }

    void FlipTowards(Vector2 target)
    {
        Vector3 scale = transform.localScale;
        float originalScaleX = Mathf.Abs(scale.x);

        if (target.x > transform.position.x)
        {
            scale.x = originalScaleX;
        }
        else
        {
            scale.x = -originalScaleX;
        }
        transform.localScale = scale;
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Vector2 targetPosition = new Vector2(patrolPoints[destPoint].position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.2f)
        {
            destPoint = (destPoint + 1) % patrolPoints.Length;
            FlipTowards(patrolPoints[destPoint].position);
        }
    }

    void ChasePlayer()
    {
        FlipTowards(player.position);
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        FlipTowards(player.position);
        if (Time.time >= nextAttackTime)
        {
            StartCoroutine(AttackCoroutine());
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void ReturnToPatrolPath()
    {
        if (patrolPoints.Length == 0)
        {
            currentState = State.Patrolling;
            return;
        }

        Vector2 targetPosition = new Vector2(patrolPoints[destPoint].position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        FlipTowards(targetPosition);

        if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.2f)
        {
            currentState = State.Patrolling;
        }
    }

    IEnumerator AttackCoroutine()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDamageDelay);

        if (IsPlayerInAttackRange())
        {
            player.GetComponent<CharacterStats>()?.TakeDamage(attackDamage);
        }
    }

    public override void Die()
    {
        base.Die();
        animator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().simulated = false;
        }
        this.enabled = false;
        Destroy(gameObject, 2f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}