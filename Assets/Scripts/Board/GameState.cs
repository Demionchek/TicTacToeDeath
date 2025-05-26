using System;
using System.Collections.Generic;
using Interfaces;

public enum Player { None, X, O }

public struct Move
{
    public (int, int) Slot;     // Слот малой доски (например, (0, 0) — верхний левый)
    public (int, int) Field;    // Поле внутри малой доски (например, (1, 1) — центр)
    public Player Player;       // Игрок, который сделал ход
}

public class GameState : IGameState
{
    // Массив 9 малых досок (каждая 3x3)
    private SmallBoard[,] _globalBoard = new SmallBoard[3, 3];

    private Player _currentPlayer = Player.X;

    private Move? _lastMove = null;

    public Player? GlobalWinner { get; private set; } = null;

    public GameState()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                _globalBoard[i, j] = new SmallBoard();
    }

    // Копирование состояния (для MCTS)
    public GameState Clone()
    {
        var clone = new GameState();
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                clone._globalBoard[i, j] = new SmallBoard
                {
                    Cells = (Player[,])_globalBoard[i, j].Cells.Clone(),
                    IsCompleted = _globalBoard[i, j].IsCompleted,
                    Winner = _globalBoard[i, j].Winner
                };
            }
        clone._currentPlayer = _currentPlayer;
        clone._lastMove = _lastMove;
        clone.GlobalWinner = GlobalWinner;
        return clone;
    }

    // Получение последнего хода
    public Move GetLastMove()
    {
        if (_lastMove.HasValue)
            return _lastMove.Value;
        throw new InvalidOperationException("No moves have been made yet.");
    }

    // Проверка победы на глобальной доске
    public bool CheckGlobalWin(Player player)
    {
        // Проверка строк
        for (int i = 0; i < 3; i++)
        {
            if (_globalBoard[i, 0].Winner == player && _globalBoard[i, 1].Winner == player && _globalBoard[i, 2].Winner == player)
                return true;
        }
        // Проверка столбцов
        for (int j = 0; j < 3; j++)
        {
            if (_globalBoard[0, j].Winner == player && _globalBoard[1, j].Winner == player && _globalBoard[2, j].Winner == player)
                return true;
        }
        // Проверка диагоналей
        if (_globalBoard[0, 0].Winner == player && _globalBoard[1, 1].Winner == player && _globalBoard[2, 2].Winner == player)
            return true;
        if (_globalBoard[0, 2].Winner == player && _globalBoard[1, 1].Winner == player && _globalBoard[2, 0].Winner == player)
            return true;
        return false;
    }

    // Получение доступных ходов
    public List<Move> GetLegalMoves()
    {
        var moves = new List<Move>();
        (int, int) targetQuadrant = _lastMove.HasValue ? _lastMove.Value.Field : (-1, -1);

        // Если LastMove ведет на завершенную доску, разрешаем любой квадрант
        if (targetQuadrant != (-1, -1) && _globalBoard[targetQuadrant.Item1, targetQuadrant.Item2].IsCompleted)
        {
            targetQuadrant = (-1, -1); // Разрешаем любой квадрант
        }

        // Перебор всех возможных ходов
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                // Если целевой квадрант не задан, выбираем все незавершенные доски
                if (targetQuadrant == (-1, -1) || (i == targetQuadrant.Item1 && j == targetQuadrant.Item2))
                {
                    if (!_globalBoard[i, j].IsCompleted)
                    {
                        for (int x = 0; x < 3; x++)
                            for (int y = 0; y < 3; y++)
                            {
                                if (_globalBoard[i, j].Cells[x, y] == Player.None)
                                {
                                    moves.Add(new Move { Slot = (i, j), Field = (x, y), Player = _currentPlayer });
                                }
                            }
                    }
                }
            }
        return moves;
    }

    // Выполнение хода
    public void MakeMove(Move move)
    {
        var quadrant = move.Slot;
        var cell = move.Field;
        _globalBoard[quadrant.Item1, quadrant.Item2].Update(cell, move.Player);
        _lastMove = move;
        _currentPlayer = _currentPlayer == Player.X ? Player.O : Player.X;

        // Проверка победы на глобальной доске
        if (CheckGlobalWin(move.Player))
        {
            GlobalWinner = move.Player;
        }
    }
}
