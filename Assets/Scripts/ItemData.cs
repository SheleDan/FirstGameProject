using System.IO.Enumeration;
using UnityEngine;

public enum ItemType
{
    Resource,
    Consumable,
    Equipment
}

public enum EquipmentSlotType
{
    None,
    Head,
    Body,
    Weapon,
    Boots
}

[CreateAssetMenu(
    fileName = "New Item",
    menuName = "Game/Items/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField, TextArea] private string description;

    [SerializeField] private ItemType itemType;
    [SerializeField] private EquipmentSlotType equipmentSlot;

    public string ItemName => itemName;
    public Sprite Icon => icon;
    public string Description => description;
    public ItemType ItemType => itemType;
    public EquipmentSlotType EquipmentSlot => equipmentSlot;
    public bool IsEquippable =>
        itemType == ItemType.Equipment &&
        equipmentSlot != EquipmentSlotType.None;
}