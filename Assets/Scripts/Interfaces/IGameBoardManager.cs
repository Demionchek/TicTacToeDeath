namespace Interfaces
{
    public interface IGameBoardManager
    {
        void Initialize();
        void UpdateField(BoardCoordinate coord, Player player);
    }
}