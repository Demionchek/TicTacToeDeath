namespace Interfaces
{
    public interface IField
    {
        public Player Player { get; }
        public BoardCoordinate Coordinate { get; }
        public void SetField(Player player);
        public void SetBoardCoordinate(BoardCoordinate boardCoordinate);
        public void SwitchColliderActive(bool isActive);
    }
}