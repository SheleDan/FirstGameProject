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

    [Header("Interaction")]
    [SerializeField, Min(0.1f)] private float interactionDistance = 1.5f;
    [SerializeField] private LayerMask interactionLayers = ~0;
    
    private Rigidbody2D _playerRigidBody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _moveInput;
    private InputAction _interactAction;
    
    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        if (_animator != null)
        {
            _spriteRenderer = _animator.GetComponent<SpriteRenderer>();
        }

        _interactAction = new InputAction("Interact", InputActionType.Button);
        _interactAction.AddBinding("<Keyboard>/e");
        _interactAction.AddBinding("<Gamepad>/buttonNorth");
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        _interactAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        _interactAction.Disable();
    }

    private void Update()
    {
        Inventory nearbyInventory = FindNearbyInventory();
        InventoryUI inventoryUI = InventoryUI.Instance;

        if (inventoryUI != null)
        {
            inventoryUI.SetInteractionAvailable(nearbyInventory != null);

            if (_interactAction.WasPressedThisFrame())
            {
                if (inventoryUI.IsOpen)
                {
                    inventoryUI.Hide();
                }
                else if (nearbyInventory != null)
                {
                    inventoryUI.Show(nearbyInventory);
                }
            }
        }

        _moveInput = inventoryUI != null && inventoryUI.IsOpen
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

    private void OnDestroy()
    {
        _interactAction?.Dispose();
    }

    private Inventory FindNearbyInventory()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            interactionDistance,
            interactionLayers);

        Inventory closestInventory = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            Inventory inventory = hit.GetComponentInParent<Inventory>();
            if (inventory == null)
            {
                continue;
            }

            float distance = ((Vector2)inventory.transform.position -
                              (Vector2)transform.position).sqrMagnitude;
            if (distance >= closestDistance)
            {
                continue;
            }

            closestInventory = inventory;
            closestDistance = distance;
        }

        return closestInventory;
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
