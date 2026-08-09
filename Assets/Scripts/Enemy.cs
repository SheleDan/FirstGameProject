using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 0.9f;
    [SerializeField, Min(0.1f)] private float detectionRadius = 5f;
    [SerializeField, Min(0.1f)] private float loseTargetRadius = 7f;

    [Header("Attack")] 
    [SerializeField, Min(0.1f)] private float attackDistance = 1.1f;
    [SerializeField, Min(1)] private int attackDamage = 10;
    [SerializeField, Min(0.1f)] private float attackCooldown = 1.2f;

    private Rigidbody2D _enemyRigidbody;
    private Health _health;
    private bool _isChasing;
    private Health _targetHealth;
    private float _nextAttackTime;

    private void Awake()
    {
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();

        if (!target)
        {
            Player player = FindAnyObjectByType<Player>();
            if (player)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: игрок на сцене не найден.");
            }
        }

        if (target)
        {
            _targetHealth = target.GetComponent<Health>();
        }
    }

    private void FixedUpdate()
    {
        if (!target || !_health || _health.IsDead)
        {
            StopMoving();
            return;
        }
        
        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;
        
        if (!_isChasing)
        {
            if (distance > detectionRadius)
            {
                StopMoving();
                return;
            }
            
            _isChasing = true;
        }
        else if (distance > loseTargetRadius)
        {
            _isChasing = false;
            StopMoving();
            return;
        }

        if (distance <= attackDistance)
        {
            TryAttack();
        }
        
        if (distance <= stoppingDistance)
        {
            StopMoving();
            return;
        }
        
        _enemyRigidbody.linearVelocity = direction.normalized * moveSpeed;
    }

    private void StopMoving()
    {
        _enemyRigidbody.linearVelocity = Vector2.zero;
    }
    
    public void HandleDeath()
    {
        StopMoving();
        
        Debug.Log($"{gameObject.name} уничтожен.");
        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseTargetRadius);
    }

    private void TryAttack()
    {
        if (!_targetHealth || _targetHealth.IsDead || Time.time < _nextAttackTime)
        {
            return;
        }

        _nextAttackTime = Time.time + attackCooldown;
        _targetHealth.TakeDamage(attackDamage);
        
        Debug.Log($"{gameObject.name} атакует игрока.");
    }
}
