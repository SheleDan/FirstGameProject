using System;
using System.Collections.Generic;
using UnityEngine;
public class PlayerEquipment : MonoBehaviour
{
    [Serializable]
    public class EquipmentSlot
    {
        [SerializeField] private EquipmentSlotType type;
        [SerializeField] private ItemData item;

        public EquipmentSlotType Type => type;
        public ItemData Item => item;

        internal void SetItem(ItemData newItem)
        {
            item = newItem;
        }
    }

    [SerializeField] private List<EquipmentSlot> slots = new();

    public IReadOnlyList<EquipmentSlot> Slots => slots;
    public event Action Changed;

    public bool TryEquip(ItemData item, Inventory inventory)
    {
        if (item == null || !item.IsEquippable || inventory == null)
        {
            return false;
        }

        EquipmentSlot targetSlot = FindSlot(item.EquipmentSlot);
        if (targetSlot == null)
        {
            return false;
        }
        if (!inventory.RemoveItem(item))
        {
            return false;
        }

        ItemData previousItem = targetSlot.Item;
        if (previousItem != null && !inventory.AddItem(previousItem))
        {
            inventory.AddItem(item);
            return false;
        }

        targetSlot.SetItem(item);
        Changed?.Invoke();
        return true;
    }

    public bool TryUnequip(
        EquipmentSlotType slotType,
        Inventory inventory)
    {
        EquipmentSlot slot = FindSlot(slotType);
        if (slot == null || slot.Item == null || inventory == null)
        {
            return false;
        }

        if (!inventory.AddItem(slot.Item))
        {
            return false;
        }

        slot.SetItem(null);
        Changed?.Invoke();
        return true;
    }

    public ItemData GetItem(EquipmentSlotType slotType)
    {
        return FindSlot(slotType)?.Item;
    }

    private EquipmentSlot FindSlot(EquipmentSlotType type)
    {
        return slots.Find(slot => slot.Type == type);
    }
}
