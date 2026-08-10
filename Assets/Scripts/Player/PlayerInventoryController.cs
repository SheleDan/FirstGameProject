using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private InputActionReference inventoryAction;
    [SerializeField] private GameObject inventoryWindow;

    public bool IsOpen => inventoryWindow != null && inventoryWindow.activeSelf;

    private void Awake()
    {
        inventoryWindow.SetActive(false);
    }

    private void OnEnable()
    {
        inventoryAction.action.performed += ToggleInventory;
        inventoryAction.action.Enable();
    }

    private void OnDisable()
    {
        inventoryAction.action.performed -= ToggleInventory;
        inventoryAction.action.Disable();
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        inventoryWindow.SetActive(!inventoryWindow.activeSelf);
    }
}