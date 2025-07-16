using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerCombat : CharacterStats
{
    [Header("Player Specific")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public CharacterHealthUI healthUI;

    [Header("Attack Stats")]
    private int normalAttackDamage = 1;
    private int holdAttackDamage = 2;
    private int attackComboCounter = 0;

    [Header("Attack Cooldown")]
    public float attackCooldown = 0.5f;
    private float nextAttackTime = 0f;

    [Header("Invincibility")]
    public float invincibilityDuration = 1.5f;
    private bool isInvincible = false;
    public float invincibilityBlinkDelay = 0.2f;

    private AudioData audioData;

    void Awake()
    {
        audioData = Resources.Load<AudioData>("AudioData");
    }

    public override void Start()
    {
        base.Start();
        if (healthUI != null)
        {
            healthUI.UpdateHealth();
        }
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                NormalAttack();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                HoldAttack();
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if (isInvincible) return;

        base.TakeDamage(damage);

        if (healthUI != null)
        {
            healthUI.UpdateHealth();
        }

        if (AudioManager.Instance != null && audioData != null)
        {
            AudioClip clip = audioData.audioItems.FirstOrDefault(_clip => _clip.audioName == "PlayerTakeDamage").audioClip;
            if (clip != null)
            {
                AudioManager.Instance.PlaySFX(clip);
            }
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        float endTime = Time.time + invincibilityDuration;
        while (Time.time < endTime)
        {
            if (sprite != null) sprite.enabled = false;
            yield return new WaitForSeconds(invincibilityBlinkDelay);
            if (sprite != null) sprite.enabled = true;
            yield return new WaitForSeconds(invincibilityBlinkDelay);
        }

        if (sprite != null) sprite.enabled = true;
        isInvincible = false;
    }

    void NormalAttack()
    {
        attackComboCounter++;
        if (attackComboCounter > 4) attackComboCounter = 1;
        nextAttackTime = Time.time + attackCooldown;
        animator.SetTrigger("NormalAttack");

        if (AudioManager.Instance != null && audioData != null)
        {
            AudioClip clip = audioData.audioItems.FirstOrDefault(_clip => _clip.audioName == "Attack1").audioClip;
            if (clip != null)
            {
                AudioManager.Instance.PlaySFX(clip);
            }
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<CharacterStats>()?.TakeDamage(normalAttackDamage);
        }
    }

    void HoldAttack()
    {
        nextAttackTime = Time.time + attackCooldown;
        animator.SetTrigger("HoldAttack");

        if (AudioManager.Instance != null && audioData != null)
        {
            AudioClip clip = audioData.audioItems.FirstOrDefault(_clip => _clip.audioName == "Attack2").audioClip;
            if (clip != null)
            {
                AudioManager.Instance.PlaySFX(clip);
            }
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<CharacterStats>()?.TakeDamage(holdAttackDamage);
        }
    }

    public override void Die()
    {
        base.Die();
        animator.SetBool("isDead", true);
        this.enabled = false;
        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().simulated = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}