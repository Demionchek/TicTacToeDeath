using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotController : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private GameState backButton;

    private Vector3 homeLocalPos;
    private bool isSelected;

    public static event Action<Transform> OnSlotSelected;

    private static Vector3 SELECTED_SCALE = new Vector3(3.0f, 3.0f, 1f);

    private void Awake()
    {
        homeLocalPos = transform.localPosition;
        SlotController.OnSlotSelected += HideIfNotSelected;
    }

    private void HideIfNotSelected(Transform target)
    {
        if (target == transform) return;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SlotController.OnSlotSelected -= HideIfNotSelected;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (isSelected)
        {
            SetHomeTransform();
        }
    }

    private void SetSelectedTransform()
    {
        isSelected = true;
        transform.localPosition = Vector3.zero;
        transform.localScale = SELECTED_SCALE;
    }

    public void SetHomeTransform()
    {
        transform.localPosition = homeLocalPos;
        transform.localScale = Vector3.one;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelectedTransform();
        OnSlotSelected?.Invoke(transform);
    }
}
