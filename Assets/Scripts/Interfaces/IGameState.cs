using System.Collections.Generic;

namespace Interfaces
{
    public interface IGameState
    {
        Player? GlobalWinner { get; }
        GameState Clone();
        Move GetLastMove();
        bool CheckGlobalWin(Player player);
        List<Move> GetLegalMoves();
        void MakeMove(Move move);
    }
}