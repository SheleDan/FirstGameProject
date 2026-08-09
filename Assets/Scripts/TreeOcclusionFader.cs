using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TreeOcclusionFader : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private SpriteRenderer targetRenderer;

    [Header("Fade")]
    [SerializeField, Range(0f, 1f)] private float fadedAlpha = 0.35f;
    [SerializeField, Min(0.1f)] private float fadeSpeed = 4f;

    private readonly HashSet<Collider2D> _playersInside = new();
    private float _targetAlpha = 1f;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = transform.parent != null
                ? transform.parent.GetComponentInChildren<SpriteRenderer>()
                : GetComponentInChildren<SpriteRenderer>();
        }

        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        if (targetRenderer == null)
        {
            return;
        }

        Color color = targetRenderer.color;
        color.a = Mathf.MoveTowards(
            color.a,
            _targetAlpha,
            fadeSpeed * Time.deltaTime);
        targetRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other))
        {
            return;
        }

        _playersInside.Add(other);
        _targetAlpha = fadedAlpha;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_playersInside.Remove(other))
        {
            return;
        }

        if (_playersInside.Count == 0)
        {
            _targetAlpha = 1f;
        }
    }

    private static bool IsPlayer(Collider2D other)
    {
        Rigidbody2D attachedBody = other.attachedRigidbody;

        return attachedBody != null && attachedBody.CompareTag("Player");
    }

    private void OnDisable()
    {
        _playersInside.Clear();
        _targetAlpha = 1f;

        if (targetRenderer == null)
        {
            return;
        }

        Color color = targetRenderer.color;
        color.a = 1f;
        targetRenderer.color = color;
    }
}
