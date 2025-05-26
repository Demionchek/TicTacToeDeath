namespace DefaultNamespace
{
    public static class CoordinateMapper
    {
        // Move -> BoardCoordinate
        public static BoardCoordinate MoveToCoordinate(Move move)
        {
            return new BoardCoordinate
            {
                SlotX = move.Slot.Item1,
                SlotY = move.Slot.Item2,
                FieldX = move.Field.Item1,
                FieldY = move.Field.Item2
            };
        }

        // BoardCoordinate -> Move (с указанием игрока)
        public static Move CoordinateToMove(BoardCoordinate coord, Player player)
        {
            return new Move
            {
                Slot = (coord.SlotX, coord.SlotY),
                Field = (coord.FieldX, coord.FieldY),
                Player = player
            };
        }

        // Оптимизированная версия для частых вызовов (без создания новых структур)
        public static void CopyToCoordinate(Move move, ref BoardCoordinate coord)
        {
            coord.SlotX = move.Slot.Item1;
            coord.SlotY = move.Slot.Item2;
            coord.FieldX = move.Field.Item1;
            coord.FieldY = move.Field.Item2;
        }
    }
}