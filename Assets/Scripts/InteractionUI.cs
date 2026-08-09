using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    private Text _hint;
    private Font _font;

    public static InteractionUI Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Create()
    {
        if (FindAnyObjectByType<InteractionUI>() != null)
        {
            return;
        }

        new GameObject("Interaction UI").AddComponent<InteractionUI>();
    }

    private void Awake()
    {
        if (Instance&& Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        BuildInterface();
    }

    public void SetInteractionAvailable(
        bool isAvailable,
        string interactionHint)
    {
        if (!_hint)
        {
            return;
        }

        bool shouldShow =
            isAvailable &&
            !string.IsNullOrWhiteSpace(interactionHint);

        if (shouldShow)
        {
            _hint.text = interactionHint;
        }

        _hint.gameObject.SetActive(shouldShow);
    }

    private void BuildInterface()
    {
        _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 90;

        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        GameObject hintObject = new(
            "Interaction Hint",
            typeof(RectTransform));

        hintObject.transform.SetParent(transform, false);

        _hint = hintObject.AddComponent<Text>();
        _hint.font = _font;
        _hint.fontSize = 27;
        _hint.alignment = TextAnchor.MiddleCenter;
        _hint.color = Color.white;

        RectTransform hintRect = _hint.rectTransform;
        hintRect.anchorMin = new Vector2(0.5f, 0f);
        hintRect.anchorMax = new Vector2(0.5f, 0f);
        hintRect.offsetMin = new Vector2(-360f, 70f);
        hintRect.offsetMax = new Vector2(360f, 130f);

        Outline outline = hintObject.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2f, -2f);

        _hint.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}