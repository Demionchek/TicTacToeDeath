namespace Interfaces
{
    public interface IGameBoardManager
    {
        public void UpdateField(BoardCoordinate coord, Player player);
        public IField GetField(BoardCoordinate coord);
    }
}