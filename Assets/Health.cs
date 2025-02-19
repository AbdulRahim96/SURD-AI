using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    public float maxHealth;
    public LayerMask attackableLayer;
    public UnityEvent dieEvent;

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth += damage;
        if(currentHealth <= 0)
        {
            dieEvent.Invoke();
        }
    }
}
