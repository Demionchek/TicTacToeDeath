
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

    public void ShowBackButton(Transform t) => backButton.SetActive(true);

    private void OnDestroy()
    {
        SlotController.OnSlotSelected -= ShowBackButton;
    }

    public void ShowBoards()
    {
        foreach (SlotController slot in slotControllers)
        {
            slot.Show();
        }
        backButton.SetActive(false);
    }
}
