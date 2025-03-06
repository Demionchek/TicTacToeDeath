
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    [SerializeField] private GameObject backButton;
    [SerializeField] private List<SlotController> slotControllers;

    private void Awake()
    {
        SlotController.OnSlotSelected += ShowBackButton;
    }

    public void ShowBackButton(Transform t) => gameObject.SetActive(true);

    public void ShowBoards()
    {
        foreach (SlotController slot in slotControllers)
        {
            slot.Show();
        }
    }
}
