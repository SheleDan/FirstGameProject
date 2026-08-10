using Interfaces;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Chest : MonoBehaviour, IInteractable
{
    [Header("Interaction")]
    [SerializeField] private string displayName = "Сундук";
    [SerializeField] private string interactionHint = $"Нажмите E, чтобы открыть сундук";
    
    private Inventory _inventory;
    
    public string InteractionHint => interactionHint;

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    public void Interact(Player player)
    {
        InventoryUI inventoryUI = InventoryUI.Instance;
        if (!inventoryUI)
        {
            return;
        }

        Inventory playerInventory = player.GetComponentInChildren<Inventory>();
        if (!playerInventory)
        {
            Debug.LogWarning("У игрока не найден Inventory.");
            return;
        }
        
        inventoryUI.Show(
            _inventory,
            playerInventory,
            displayName);
    }
}
