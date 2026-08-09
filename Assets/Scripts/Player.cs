using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference interactAction;

    [Header("Interaction")]
    [SerializeField] private InteractionDetector interactionDetector;
    
    private Rigidbody2D _playerRigidBody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _moveInput;
    
    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        if (_animator != null)
        {
            _spriteRenderer = _animator.GetComponent<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        interactAction.action.Disable();
    }

    private void Update()
    {
        IInteractable nearbyInteractable = interactionDetector.GetClosestInteractable(transform.position);
        InventoryUI inventoryUI = InventoryUI.Instance;
        InteractionUI interactionUI = InteractionUI.Instance;

        bool inventoryIsOpen = inventoryUI && inventoryUI.IsOpen;

        if (interactionUI)
        {
            interactionUI.SetInteractionAvailable(
                nearbyInteractable != null && !inventoryIsOpen,
                nearbyInteractable?.InteractionHint);
        }
        
        if (interactAction.action.WasPressedThisFrame())
        {
            if (inventoryUI && inventoryUI.IsOpen)
            {
                inventoryUI.Hide();
            }
            else
            {
                nearbyInteractable?.Interact(this);
            }
        }

        _moveInput = inventoryUI && inventoryUI.IsOpen
            ? Vector2.zero
            : moveAction.action.ReadValue<Vector2>();
        _moveInput = Vector2.ClampMagnitude(_moveInput, 1f);
        
        if (_moveInput.sqrMagnitude > 0.01f)
        {
            FacingDirection = GetFourDirection(_moveInput);
        }

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        _playerRigidBody.linearVelocity = _moveInput * moveSpeed;
    }

    // Фиксирует направление взгляда в одной из четырех сторон.
    private Vector2 GetFourDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector2.right : Vector2.left;
        }
        
        return direction.y > 0 ? Vector2.up : Vector2.down;
    }

    private void UpdateAnimation()
    {
        if (!_animator)
        {
            return;
        }

        _animator.SetFloat(MoveXHash, FacingDirection.x);
        _animator.SetFloat(MoveYHash, FacingDirection.y);
        _animator.SetFloat(SpeedHash, _moveInput.sqrMagnitude);

        if (_spriteRenderer&& Mathf.Abs(FacingDirection.x) > 0.01f)
        {
            _spriteRenderer.flipX = FacingDirection.x < 0f;
        }
    }
}
