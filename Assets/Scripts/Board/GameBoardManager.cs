using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Структура для хранения координат
public struct BoardCoordinate
{
    public int SlotX;    // Номер слота (квадранта) по X (0-2)
    public int SlotY;    // Номер слота (квадранта) по Y (0-2)
    public int FieldX;   // Номер поля (ячейки) внутри слота по X (0-2)
    public int FieldY;   // Номер поля (ячейки) внутри слота по Y (0-2)

    public override string ToString() => $"Slot ({SlotX},{SlotY}), Field ({FieldX},{FieldY})";
}

public class GameBoardManager : MonoBehaviour
{
    // Словарь для хранения связки координат и GameObject
    private Dictionary<BoardCoordinate, Field> _fieldObjects = new Dictionary<BoardCoordinate, Field>();

    // Событие для обработки кликов (можно вызывать из других скриптов)
    public UnityEvent<BoardCoordinate> OnFieldClicked = new UnityEvent<BoardCoordinate>();

    private void Awake()
    {
        InitializeBoard();
    }

    // Инициализация доски
    private void InitializeBoard()
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
                        Field field = fieldGO?.GetComponent<Field>();
                        if (field == null)
                        {
                            Debug.LogError($"Не найдено поле Field_{fieldX}_{fieldY} в слоте {slotX},{slotY}!");
                            continue;
                        }

                        _fieldObjects.Add(coord, field);

                        //TODO: Прокинуть GameBoardManager всем Field
                    }
            }
    }

    // Обработка клика на поле
    private void HandleFieldClick(BoardCoordinate coord)
    {
        Debug.Log($"Кликнуто: {coord}");
        OnFieldClicked.Invoke(coord);
    }

    // Получение GameObject поля по координатам
    public Field GetField(BoardCoordinate coord)
    {
        return _fieldObjects.TryGetValue(coord, out var field) ? field : null;
    }

    // Обновление визуального состояния поля
    public void UpdateFieldVisual(BoardCoordinate coord, Player player)
    {

    }
}