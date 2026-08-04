using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField, Min(1)] private int maxHealth = 100;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onDamaged;
    [SerializeField] private UnityEvent onDied;
    
    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDead { get; private set; }
    
    private  void Awake()
    {
        CurrentHealth = maxHealth;
        IsDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0)
        {
            return;
        }
        
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        
        Debug.Log($"{gameObject.name}: получил {damage} урона." +
                  $"Здоровье: {CurrentHealth}/{maxHealth}.");
        
        onDamaged?.Invoke();
        if (CurrentHealth == 0)
        {
            Die();
        }
    }
    
    public void Heal(int amount)
    {
        if (IsDead || amount <= 0)
        {
            return;
        }
        
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        
        Debug.Log($"{gameObject.name}: получил {amount} хила." +
                  $"Здоровье: {CurrentHealth}/{maxHealth}.");
    }

    private void Die()
    {
        IsDead = true;
        
        Debug.Log($"{gameObject.name} погиб.");
        
        onDied?.Invoke();
    }

    [ContextMenu("Test/Take 25 Damage")]
    private void TestDamage25()
    {
        TakeDamage(25);
    }
    
    [ContextMenu("Test/Heal 25")]
    private void TestHeal25()
    {
        Heal(25);
    }
}
