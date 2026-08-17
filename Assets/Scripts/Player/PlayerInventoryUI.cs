using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform slotsGrid;
    [SerializeField] private InventorySlotView slotPrefab;

    private void OnEnable()
    {
        if (inventory == null)
        {
            return;
        }

        inventory.Changed += Rebuild;
        Rebuild();
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.Changed -= Rebuild;
        }
    }

    private void Rebuild()
    {
        ClearSlots();

        foreach (Inventory.InventorySlot slot in inventory.Slots)
        {
            if (slot.Item == null || slot.Amount <= 0)
            {
                continue;
            }

            ItemData item = slot.Item;

            InventorySlotView view = Instantiate(slotPrefab, slotsGrid);

            view.Setup(item, slot.Amount, () => HandleItemClick(item));
        }
    }

    private void ClearSlots()
    {
        for (int i = slotsGrid.childCount - 1; i >= 0; i--)
        {
            Destroy(slotsGrid.GetChild(i).gameObject);
        }
    }

    private void HandleItemClick(ItemData item)
    {
        Debug.Log($"Нажат предмет: {item.ItemName}");
    }
}