using System.Collections.Generic;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractionDetector : MonoBehaviour
{
    private readonly Dictionary<IInteractable, int> _overlapCounts = new();

    public IInteractable GetClosestInteractable(Vector2 origin)
    {
        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (IInteractable interactable in _overlapCounts.Keys)
        {
            if (interactable is not Component component || !component)
            {
                continue;
            }

            float distance = (
                (Vector2)component.transform.position - origin
            ).sqrMagnitude;

            if (distance >= closestDistance)
            {
                continue;
            }

            closestInteractable = interactable;
            closestDistance = distance;
        }

        return closestInteractable;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable =
            other.GetComponentInParent<IInteractable>();

        if (interactable == null)
        {
            return;
        }

        if (_overlapCounts.TryGetValue(interactable, out int count))
        {
            _overlapCounts[interactable] = count + 1;
        }
        else
        {
            _overlapCounts.Add(interactable, 1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable =
            other.GetComponentInParent<IInteractable>();

        if (interactable == null ||
            !_overlapCounts.TryGetValue(interactable, out int count))
        {
            return;
        }

        if (count > 1)
        {
            _overlapCounts[interactable] = count - 1;
        }
        else
        {
            _overlapCounts.Remove(interactable);
        }
    }

    private void OnDisable()
    {
        _overlapCounts.Clear();
    }

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }
}