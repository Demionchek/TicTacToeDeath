namespace Interfaces
{
    public interface IGameState
    {
        SmallBoard[,] GlobalBoard { get; }
        Player CurrentPlayer { get; }
        Move? LastMove { get; }
        Player? GlobalWinner { get; }
        void MakeMove(Move move);
    }
}