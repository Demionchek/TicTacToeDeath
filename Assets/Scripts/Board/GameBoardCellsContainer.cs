using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class GameBoardCellsContainer : MonoBehaviour, IGameBoardCellsContainer
{
    private List<CellData> AllCells = new List<CellData>();
    public List<CellData> GetListCells(List<CellData> cells) => AllCells;
}


[System.Serializable]
public class CellData
{
    public BoardCoordinate Coordinate;
    public GameObject CellObject;
}
