using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Button button;

    public void Setup(
        ItemData item,
        int amount,
        UnityAction onClick
    )
    {
        icon.sprite = item.Icon;
        icon.enabled = item.Icon != null;

        amountText.text = amount > 1 ? amount.ToString() : string.Empty;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClick);
    }
}