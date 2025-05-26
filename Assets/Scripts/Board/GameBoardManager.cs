using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Interfaces;
using MCTS;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public struct BoardCoordinate : IEquatable<BoardCoordinate>
{
    public int SlotX;    // Номер слота (квадранта) по X (0-2)
    public int SlotY;    // Номер слота (квадранта) по Y (0-2)
    public int FieldX;   // Номер поля (ячейки) внутри слота по X (0-2)
    public int FieldY;   // Номер поля (ячейки) внутри слота по Y (0-2)

    public override string ToString() => $"Slot ({SlotX},{SlotY}), Field ({FieldX},{FieldY})";

    public bool Equals(BoardCoordinate other)
    {
        return
            SlotX == other.SlotX &&
            SlotY == other.SlotY &&
            FieldX == other.FieldX &&
            FieldY == other.FieldY;
    }
}

public class GameBoardManager : MonoBehaviour, IGameBoardManager
{

    private Dictionary<BoardCoordinate, IField> _fieldObjects = new Dictionary<BoardCoordinate, IField>();
    public Dictionary<BoardCoordinate, IField> FieldObjects  { get => _fieldObjects;}

    public UnityEvent<BoardCoordinate> OnFieldClicked = new UnityEvent<BoardCoordinate>();

    [Inject]
    private IGameState _gameState;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        for (int slotX = 0; slotX < 3; slotX++)
            for (int slotY = 0; slotY < 3; slotY++)
            {
                Transform slotTransform = GameObject.Find($"Slot_{slotX}_{slotY}")?.transform;
                if (slotTransform == null)
                {
                    Debug.LogError($"Не найден объект Slot_{slotX}_{slotY}!");
                    continue;
                }

                for (int fieldX = 0; fieldX < 3; fieldX++)
                    for (int fieldY = 0; fieldY < 3; fieldY++)
                    {
                        BoardCoordinate coord = new BoardCoordinate
                        {
                            SlotX = slotX,
                            SlotY = slotY,
                            FieldX = fieldX,
                            FieldY = fieldY
                        };

                        GameObject fieldGO = slotTransform.Find($"Field_{fieldX}_{fieldY}")?.gameObject;
                        IField field = fieldGO?.GetComponent<IField>();
                        if (field == null)
                        {
                            Debug.LogError($"Не найдено поле Field_{fieldX}_{fieldY} в слоте {slotX},{slotY}!");
                            continue;
                        }

                        _fieldObjects.Add(coord, field);

                        field.SetBoardCoordinate(coord);
                    }
            }
    }

    public void UpdateField(BoardCoordinate coord, Player player)
    {
        _fieldObjects.TryGetValue(coord, out IField field);

        if (field == null)
        {
            Debug.LogError("UpdateField: No field found! coord is " + coord);
            return;
        }

        field.SetField(player);

        if (player == Player.O) HandleFieldClick(coord);
    }

    public void HandleFieldClick(BoardCoordinate coord)
    {
        OnFieldClicked.Invoke(coord);

        Move playerMove = CoordinateMapper.CoordinateToMove(coord, Player.O);
        _gameState.MakeMove(playerMove);
        MakeAiMove();
    }

    public void MakeAiMove()
    {
        MctsAlgorithm mctsAlgorithm = new MctsAlgorithm(_gameState, 2000, false);
        Move aiMove = mctsAlgorithm.FindBestMove();
        BoardCoordinate coordinate = CoordinateMapper.MoveToCoordinate(aiMove);
        UpdateField(coordinate, Player.X);
    }

    public IField GetField(BoardCoordinate coord)
    {
        return _fieldObjects.TryGetValue(coord, out var field) ? field : null;
    }
}