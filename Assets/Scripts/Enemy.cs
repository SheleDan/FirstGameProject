using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 0.9f;

    private Rigidbody2D _enemyRigidbody;
    private Health _health;

    private void Awake()
    {
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();

        if (!target)
        {
            Player player = FindFirstObjectByType<Player>();
            if (player)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning($"{gameObject.name}: игрок на сцене не найден.");
            }
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
}
