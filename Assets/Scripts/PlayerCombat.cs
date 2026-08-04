using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference attackAction;
    
    [Header("Attack")]
    [SerializeField] private Transform attackPoint;

    [SerializeField] private float attackDistance = 0.8f;
    [SerializeField] private float attackRadius = 0.6f;
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private float attackCooldown = 0.4f;
    [SerializeField] private LayerMask targetLauers;

    private Player _player;
    private Health _ownHealth;
    private float _nextAttackTime;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _ownHealth = _player.GetComponent<Health>();
    }

    private void OnEnable()
    {
        attackAction.action.Enable();
    }

    private void OnDisable()
    {
        attackAction.action.Disable();
    }

    private void Update()
    {
        UpdateAttackPoint();

        if (attackAction.action.WasPressedThisFrame())
        {
            Attack();
        }
    }

    private void UpdateAttackPoint()
    {
        if (!attackPoint)
        {
            return;
        }

        attackPoint.localPosition = _player.FacingDirection * attackDistance;
    }

    private void Attack()
    {
        if (!attackPoint || Time.time < _nextAttackTime)
        {
            return;
        }

        _nextAttackTime = Time.time + attackCooldown;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRadius,
            targetLauers);
        
        HashSet<Health> damagedTargets = new HashSet<Health>();

        foreach (Collider2D hit in hits)
        {
            Health targetHealth = hit.GetComponentInParent<Health>();
            if (!targetHealth || targetHealth == _ownHealth || damagedTargets.Contains(targetHealth))
            {
                continue;
            }

            targetHealth.TakeDamage(attackDamage);
            damagedTargets.Add(targetHealth);
        }
        
        Debug.Log("Player атакует");
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
