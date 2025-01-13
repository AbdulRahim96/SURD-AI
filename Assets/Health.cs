using UnityEngine;

public class Health : MonoBehaviour
{
    private float currentHealth;
    public float maxHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth += damage;
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
