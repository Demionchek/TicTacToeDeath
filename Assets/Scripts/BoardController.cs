using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardController : MonoBehaviour, IPointerClickHandler
{
    public static event Action<Transform> OnBoardSelected;
    private Vector3 homeLocalPos;

    private static Vector3 SELECTED_SCALE = new Vector3(3.0f, 3.0f, 3.0f);

    private void Awake()
    {
        homeLocalPos = transform.localPosition;
    }

    private void SetSelectedTransform()
    {
        transform.localPosition = homeLocalPos;
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
        OnBoardSelected?.Invoke(transform);
    }
}
