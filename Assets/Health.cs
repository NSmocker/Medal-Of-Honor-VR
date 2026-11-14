using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth=100f;
    
    public UnityEvent onDeath;
    public UnityEvent onDamageTaken;
    public UnityEvent onHealed;

    public bool isDead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        if (currentHealth <= 0)
        {   isDead = true;
            onDeath?.Invoke();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        onDamageTaken?.Invoke();
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        onHealed?.Invoke();
    }
    
}
