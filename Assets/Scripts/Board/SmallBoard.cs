public class SmallBoard
{

    public Player[,] Cells { get; set; } = new Player[3, 3];
    public bool IsCompleted { get; set; } = false;
    public Player? Winner { get; set; } = null;

    // Проверка победы на малой доске
    public bool CheckWin(Player player)
    {
        // Проверка строк
        for (int i = 0; i < 3; i++)
        {
            if (Cells[i, 0] == player && Cells[i, 1] == player && Cells[i, 2] == player)
                return true;
        }
        // Проверка столбцов
        for (int j = 0; j < 3; j++)
        {
            if (Cells[0, j] == player && Cells[1, j] == player && Cells[2, j] == player)
                return true;
        }
        // Проверка диагоналей
        if (Cells[0, 0] == player && Cells[1, 1] == player && Cells[2, 2] == player)
            return true;
        if (Cells[0, 2] == player && Cells[1, 1] == player && Cells[2, 0] == player)
            return true;
        return false;
    }

    // Проверка заполненности доски
    public bool IsFull()
    {
        for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
        {
            if (Cells[i, j] == Player.None)
                return false;
        }
        return true;
    }

    // Обновление состояния доски
    public void Update((int, int) cell, Player player)
    {
        Cells[cell.Item1, cell.Item2] = player;
        if (CheckWin(player))
        {
            Winner = player;
            IsCompleted = true;
        }
        else if (IsFull())
        {
            IsCompleted = true;
        }
    }
}
