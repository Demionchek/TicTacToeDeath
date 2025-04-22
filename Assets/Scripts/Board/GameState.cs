using System;
using System.Collections.Generic;
using Interfaces;

public enum Player { None, X, O }

public struct Move
{
    public (int, int) Quadrant; // Квадрант малой доски (например, (0, 0) — верхний левый)
    public (int, int) Cell;     // Ячейка внутри малой доски (например, (1, 1) — центр)
    public Player Player;       // Игрок, который сделал ход
}

public class GameState : IGameState
{
    // Массив 9 малых досок (каждая 3x3)
    public SmallBoard[,] GlobalBoard { get; private set; } = new SmallBoard[3, 3];

    // Текущий игрок (X или O)
    public Player CurrentPlayer { get; private set; } = Player.X;

    // Последний сделанный ход
    public Move? LastMove { get; private set; } = null;

    // Победитель глобальной доски
    public Player? GlobalWinner { get; private set; } = null;

    // Инициализация малых досок
    public GameState()
    {
        for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
            GlobalBoard[i, j] = new SmallBoard();
    }

    // Копирование состояния (для MCTS)
    public GameState Clone()
    {
        var clone = new GameState();
        for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
        {
            clone.GlobalBoard[i, j] = new SmallBoard
            {
                Cells = (Player[,])GlobalBoard[i, j].Cells.Clone(),
                IsCompleted = GlobalBoard[i, j].IsCompleted,
                Winner = GlobalBoard[i, j].Winner
            };
        }
        clone.CurrentPlayer = CurrentPlayer;
        clone.LastMove = LastMove;
        clone.GlobalWinner = GlobalWinner;
        return clone;
    }

    // Получение последнего хода
    public Move GetLastMove()
    {
        if (LastMove.HasValue)
            return LastMove.Value;
        throw new InvalidOperationException("No moves have been made yet.");
    }

    // Проверка победы на глобальной доске
    public bool CheckGlobalWin(Player player)
    {
        // Проверка строк
        for (int i = 0; i < 3; i++)
        {
            if (GlobalBoard[i, 0].Winner == player && GlobalBoard[i, 1].Winner == player && GlobalBoard[i, 2].Winner == player)
                return true;
        }
        // Проверка столбцов
        for (int j = 0; j < 3; j++)
        {
            if (GlobalBoard[0, j].Winner == player && GlobalBoard[1, j].Winner == player && GlobalBoard[2, j].Winner == player)
                return true;
        }
        // Проверка диагоналей
        if (GlobalBoard[0, 0].Winner == player && GlobalBoard[1, 1].Winner == player && GlobalBoard[2, 2].Winner == player)
            return true;
        if (GlobalBoard[0, 2].Winner == player && GlobalBoard[1, 1].Winner == player && GlobalBoard[2, 0].Winner == player)
            return true;
        return false;
    }

    // Получение доступных ходов
    public List<Move> GetLegalMoves()
    {
        var moves = new List<Move>();
        (int, int) targetQuadrant = LastMove.HasValue ? LastMove.Value.Cell : (-1, -1);

        // Если LastMove ведет на завершенную доску, разрешаем любой квадрант
        if (targetQuadrant != (-1, -1) && GlobalBoard[targetQuadrant.Item1, targetQuadrant.Item2].IsCompleted)
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
                if (!GlobalBoard[i, j].IsCompleted)
                {
                    for (int x = 0; x < 3; x++)
                    for (int y = 0; y < 3; y++)
                    {
                        if (GlobalBoard[i, j].Cells[x, y] == Player.None)
                        {
                            moves.Add(new Move { Quadrant = (i, j), Cell = (x, y), Player = CurrentPlayer });
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
        var quadrant = move.Quadrant;
        var cell = move.Cell;
        GlobalBoard[quadrant.Item1, quadrant.Item2].Update(cell, move.Player);
        LastMove = move;
        CurrentPlayer = CurrentPlayer == Player.X ? Player.O : Player.X;

        // Проверка победы на глобальной доске
        if (CheckGlobalWin(move.Player))
        {
            GlobalWinner = move.Player;
        }
    }
}
