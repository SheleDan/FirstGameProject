using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private static readonly Color PanelColor = new(0.09f, 0.08f, 0.07f, 0.95f);
    private static readonly Color SlotColor = new(0.22f, 0.20f, 0.17f, 1f);

    private GameObject _panel;
    private RectTransform _content;
    private Text _title;
    private Text _hint;
    private Inventory _currentInventory;
    private Font _font;

    public static InventoryUI Instance { get; private set; }
    public bool IsOpen => _panel != null && _panel.activeSelf;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Create()
    {
        if (FindAnyObjectByType<InventoryUI>() != null)
        {
            return;
        }

        new GameObject("Inventory UI").AddComponent<InventoryUI>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        BuildInterface();
    }

    private void Update()
    {
        if (IsOpen && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Hide();
        }
    }

    public void Show(Inventory inventory, string ownerName = "Сундук")
    {
        if (inventory == null)
        {
            return;
        }

        UnsubscribeFromCurrentInventory();
        _currentInventory = inventory;
        _currentInventory.Changed += RebuildSlots;

        _title.text = string.IsNullOrWhiteSpace(ownerName) ? "Сундук" : ownerName;
        _panel.SetActive(true);
        _hint.gameObject.SetActive(false);
        RebuildSlots();
    }

    public void Hide()
    {
        if (_panel == null)
        {
            return;
        }

        _panel.SetActive(false);
        UnsubscribeFromCurrentInventory();
    }

    public void SetInteractionAvailable(bool isAvailable)
    {
        if (_hint != null)
        {
            _hint.gameObject.SetActive(isAvailable && !IsOpen);
        }
    }

    private void BuildInterface()
    {
        _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;
        gameObject.AddComponent<GraphicRaycaster>();

        _panel = CreateUiObject("Inventory Panel", transform);
        RectTransform panelRect = _panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(560f, 520f);

        Image panelImage = _panel.AddComponent<Image>();
        panelImage.color = PanelColor;

        _title = CreateText("Title", _panel.transform, 34, TextAnchor.MiddleLeft);
        SetRect(_title.rectTransform, new Vector2(32f, -88f), new Vector2(-100f, -24f),
            new Vector2(0f, 1f), new Vector2(1f, 1f));

        Button closeButton = CreateButton(_panel.transform);
        closeButton.onClick.AddListener(Hide);

        GameObject contentObject = CreateUiObject("Slots", _panel.transform);
        _content = contentObject.GetComponent<RectTransform>();
        SetRect(_content, new Vector2(28f, 28f), new Vector2(-28f, -96f),
            Vector2.zero, Vector2.one);

        VerticalLayoutGroup layout = contentObject.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 10f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = true;

        _hint = CreateText("Interaction Hint", transform, 27, TextAnchor.MiddleCenter);
        _hint.text = "Нажмите E, чтобы открыть сундук";
        _hint.color = Color.white;
        SetRect(_hint.rectTransform, new Vector2(-360f, 70f), new Vector2(360f, 130f),
            new Vector2(0.5f, 0f), new Vector2(0.5f, 0f));

        Outline hintOutline = _hint.gameObject.AddComponent<Outline>();
        hintOutline.effectColor = Color.black;
        hintOutline.effectDistance = new Vector2(2f, -2f);

        _panel.SetActive(false);
        _hint.gameObject.SetActive(false);
    }

    private void RebuildSlots()
    {
        for (int i = _content.childCount - 1; i >= 0; i--)
        {
            Destroy(_content.GetChild(i).gameObject);
        }

        if (_currentInventory == null || _currentInventory.Slots.Count == 0)
        {
            CreateSlotText("Сундук пуст");
            return;
        }

        foreach (Inventory.InventorySlot slot in _currentInventory.Slots)
        {
            CreateSlotText($"{slot.ItemId}   x{slot.Amount}");
        }
    }

    private void CreateSlotText(string value)
    {
        GameObject slotObject = CreateUiObject("Slot", _content);
        slotObject.AddComponent<Image>().color = SlotColor;

        LayoutElement layoutElement = slotObject.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = 62f;
        layoutElement.minHeight = 62f;

        Text text = CreateText("Label", slotObject.transform, 28, TextAnchor.MiddleLeft);
        text.text = value;
        SetRect(text.rectTransform, new Vector2(18f, 0f), new Vector2(-18f, 0f),
            Vector2.zero, Vector2.one);
    }

    private Button CreateButton(Transform parent)
    {
        GameObject buttonObject = CreateUiObject("Close Button", parent);
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        SetRect(rect, new Vector2(-88f, -86f), new Vector2(-24f, -22f),
            Vector2.one, Vector2.one);

        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.42f, 0.18f, 0.14f, 1f);

        Button button = buttonObject.AddComponent<Button>();
        Text label = CreateText("Label", buttonObject.transform, 30, TextAnchor.MiddleCenter);
        label.text = "X";
        SetRect(label.rectTransform, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one);
        return button;
    }

    private Text CreateText(string objectName, Transform parent, int fontSize, TextAnchor alignment)
    {
        GameObject textObject = CreateUiObject(objectName, parent);
        Text text = textObject.AddComponent<Text>();
        text.font = _font;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        return text;
    }

    private static GameObject CreateUiObject(string objectName, Transform parent)
    {
        GameObject result = new(objectName, typeof(RectTransform));
        result.transform.SetParent(parent, false);
        return result;
    }

    private static void SetRect(
        RectTransform rect,
        Vector2 offsetMin,
        Vector2 offsetMax,
        Vector2 anchorMin,
        Vector2 anchorMax)
    {
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
    }

    private void UnsubscribeFromCurrentInventory()
    {
        if (_currentInventory != null)
        {
            _currentInventory.Changed -= RebuildSlots;
            _currentInventory = null;
        }
    }

    private void OnDestroy()
    {
        UnsubscribeFromCurrentInventory();

        if (Instance == this)
        {
            Instance = null;
        }
    }
}
