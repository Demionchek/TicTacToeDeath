using System.Collections.Generic;

namespace Interfaces
{
    public interface IGameBoardCellsContainer
    {
        public List<CellData> GetListCells(List<CellData> cells);
    }
}