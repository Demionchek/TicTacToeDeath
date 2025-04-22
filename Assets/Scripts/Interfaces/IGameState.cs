namespace Interfaces
{
    public interface IGameState
    {
        void MakeMove(Move move);
        Player CurrentPlayer { get; }
        Player? GlobalWinner { get; }
        SmallBoard[,] GlobalBoard { get; }
    }
}