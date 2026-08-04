using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveAction;
    
    private Rigidbody2D _playerRigidBody;
    private Vector2 _moveInput;
    
    public Vector2 FacingDirection { get; private set; } = Vector2.zero;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        _moveInput = moveAction.action.ReadValue<Vector2>();
        _moveInput = Vector2.ClampMagnitude(_moveInput, 1f);
        
        if (_moveInput.sqrMagnitude > 0.01f)
        {
            FacingDirection = GetFourDirection(_moveInput);
        }
    }

    private void FixedUpdate()
    {
        _playerRigidBody.linearVelocity = _moveInput * moveSpeed;
    }

    // Фиксирует направление взгляда в одлной из четырех сторон.
    private Vector2 GetFourDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector2.right : Vector2.left;
        }
        
        return direction.y > 0 ? Vector2.up : Vector2.down;
    }
}
