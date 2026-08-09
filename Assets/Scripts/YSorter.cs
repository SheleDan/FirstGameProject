using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
[RequireComponent(typeof(SortingGroup))]
public class YSorter : MonoBehaviour
{
    [Header("Sorting")]
    [SerializeField] private Transform sortPoint;
    [SerializeField] private int orderOffset;
    [SerializeField] private int precision = 100;

    private SortingGroup _sortingGroup;

    private void Awake()
    {
        _sortingGroup = GetComponent<SortingGroup>();
    }

    private void OnEnable()
    {
        UpdateSortingOrder();
    }

    private void OnValidate()
    {
        UpdateSortingOrder();
    }

    private void LateUpdate()
    {
        UpdateSortingOrder();
    }

    private void UpdateSortingOrder()
    {
        if (_sortingGroup == null)
        {
            _sortingGroup = GetComponent<SortingGroup>();
        }

        Vector3 position = sortPoint
            ? sortPoint.position
            : transform.position;

        _sortingGroup.sortingOrder =
            orderOffset - Mathf.RoundToInt(position.y * precision);
    }
}