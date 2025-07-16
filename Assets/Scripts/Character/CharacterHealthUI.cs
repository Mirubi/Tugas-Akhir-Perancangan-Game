using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthUI : MonoBehaviour
{
    public Image healthBarFill;
    public CharacterStats targetStats;

    public Color fullHealthColor = Color.green;
    public Color midHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;
    public bool useColorChange = true;

    void Awake()
    {
        if (targetStats == null)
        {
            targetStats = GetComponentInParent<CharacterStats>();
        }
    }

    public void UpdateHealth()
    {
        if (targetStats == null || healthBarFill == null) return;

        float currentHealth = targetStats.GetCurrentHealth();
        float maxHealth = targetStats.maxHealth;

        float fillAmount = currentHealth / maxHealth;
        healthBarFill.fillAmount = fillAmount;

        if (useColorChange)
        {
            if (fillAmount > 0.6f)
            {
                healthBarFill.color = fullHealthColor;
            }
            else if (fillAmount > 0.3f)
            {
                healthBarFill.color = midHealthColor;
            }
            else
            {
                healthBarFill.color = lowHealthColor;
            }
        }
    }
}