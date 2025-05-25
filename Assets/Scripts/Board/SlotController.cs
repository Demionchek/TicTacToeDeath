using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotController : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Button backButton;

    private Vector3 homeLocalPos;
    private BoxCollider2D boxCollider2D;
    private bool isSelected;
    private List<IField> fields = new List<IField>();
    private static Vector3 SELECTED_SCALE = new Vector3(3.0f, 3.0f, 1f);
    public static event Action<Transform> OnSlotSelected;



    private void Awake()
    {
        homeLocalPos = transform.localPosition;

        fields = transform.GetComponentsInChildren<IField>().ToList();

        OnSlotSelected += HideIfNotSelected;
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
        if (boxCollider2D == null) boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
        transform.localPosition = Vector3.zero;
        transform.localScale = SELECTED_SCALE;
        SwitchAllFieldsCollider(isSelected);
    }

    public void SetHomeTransform()
    {
        isSelected = false;
        if (boxCollider2D == null) boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = true;
        transform.localPosition = homeLocalPos;
        transform.localScale = Vector3.one;
        SwitchAllFieldsCollider(isSelected);
    }

    private void SwitchAllFieldsCollider(bool isActive)
    {
        foreach (IField field in fields)
        {
            field.SwitchColliderActive(isActive);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelectedTransform();
        OnSlotSelected?.Invoke(transform);
    }
}
