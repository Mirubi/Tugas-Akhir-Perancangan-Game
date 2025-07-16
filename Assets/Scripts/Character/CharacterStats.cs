using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int maxHealth = 100;
    protected int currentHealth;

    public Animator animator;
    private CharacterHealthUI healthUI;

    public virtual void Start()
    {
        currentHealth = maxHealth;
        healthUI = GetComponentInChildren<CharacterHealthUI>();
        if (healthUI != null)
        {
            healthUI.UpdateHealth();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (healthUI != null)
        {
            healthUI.UpdateHealth();
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public virtual void Die()
    {
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}