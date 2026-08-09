using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [Serializable]
    public class InventorySlot
    {
        [SerializeField] private string itemId;
        [SerializeField, Min(1)] private int amount = 1;

        public string ItemId => itemId;
        public int Amount => amount;

        public InventorySlot(string itemId, int amount)
        {
            this.itemId = itemId;
            this.amount = amount;
        }

        internal void Add(int value)
        {
            amount += value;
        }
    }

    [Header("Inventory")]
    [SerializeField, Min(1)] private int capacity = 12;
    [SerializeField] private List<InventorySlot> slots = new();

    [Header("Events")]
    [SerializeField] private UnityEvent onInventoryChanged;

    public int Capacity => capacity;
    public int UsedSlots => slots.Count;
    public IReadOnlyList<InventorySlot> Slots => slots;
    public event Action Changed;

    /// <summary>
    /// Adds items to an existing stack or occupies a new slot.
    /// Returns false when the id is invalid or there are no free slots.
    /// </summary>
    public bool AddItem(string itemId, int amount = 1)
    {
        if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindSlot(itemId);
        if (slot != null)
        {
            slot.Add(amount);
            NotifyChanged();
            return true;
        }

        if (slots.Count >= capacity)
        {
            return false;
        }

        slots.Add(new InventorySlot(itemId, amount));
        NotifyChanged();
        return true;
    }

    /// <summary>
    /// Removes the requested amount. The slot disappears when its stack is empty.
    /// </summary>
    public bool RemoveItem(string itemId, int amount = 1)
    {
        if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindSlot(itemId);
        if (slot == null || slot.Amount < amount)
        {
            return false;
        }

        slot.Add(-amount);
        if (slot.Amount == 0)
        {
            slots.Remove(slot);
        }

        NotifyChanged();
        return true;
    }

    public bool HasItem(string itemId, int amount = 1)
    {
        if (string.IsNullOrWhiteSpace(itemId) || amount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindSlot(itemId);
        return slot != null && slot.Amount >= amount;
    }

    public int GetItemAmount(string itemId)
    {
        InventorySlot slot = FindSlot(itemId);
        return slot?.Amount ?? 0;
    }

    public void Clear()
    {
        if (slots.Count == 0)
        {
            return;
        }

        slots.Clear();
        NotifyChanged();
    }

    private InventorySlot FindSlot(string itemId)
    {
        return slots.Find(slot =>
            slot != null && string.Equals(slot.ItemId, itemId, StringComparison.Ordinal));
    }

    private void NotifyChanged()
    {
        Changed?.Invoke();
        onInventoryChanged?.Invoke();
    }

    private void OnValidate()
    {
        capacity = Mathf.Max(1, capacity);
    }
}
