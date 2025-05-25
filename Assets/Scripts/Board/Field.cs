using System;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Field : MonoBehaviour, IField, IPointerClickHandler
{
    private Player _player;
    private BoxCollider2D _boxCollider2D;
    private BoardCoordinate _boardCoordinate;
    [SerializeField] private GameObject _circleGo;
    [SerializeField] private GameObject _crossGo;

    [Inject]
    private IGameBoardManager _iGameBoardManager;
    public Player Player => _player;
    public BoardCoordinate Coordinate => _boardCoordinate;

    public void SetBoardCoordinate(BoardCoordinate boardCoordinate) => _boardCoordinate = boardCoordinate;

    private void OnEnable()
    {
        if (_boxCollider2D == null) _boxCollider2D = GetComponent<BoxCollider2D>();

        _boxCollider2D.enabled = false;
    }

    public void SwitchColliderActive(bool isActive)
    {
        if (_player != Player.None) return;

        _boxCollider2D.enabled = isActive;
    }

    public void SetField(Player player)
    {
        if (_player != Player.None) return;

        switch (player)
        {
            case Player.X:
                _crossGo.SetActive(true);
                break;
            case Player.O:
                _circleGo.SetActive(true);
                break;
        }

        Debug.Log($"Кликнуто: {_boardCoordinate}");

        _boxCollider2D.enabled = false;

        _player = player;
        _iGameBoardManager.UpdateField(_boardCoordinate, _player);
    }

    public void OnPointerClick(PointerEventData eventData) => SetField(Player.O);
}
