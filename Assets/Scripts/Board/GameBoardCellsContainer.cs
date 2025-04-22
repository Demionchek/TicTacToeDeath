using System.Collections.Generic;
using UnityEngine;

public class GameBoardCellsContainer : MonoBehaviour
{
    public List<CellData> AllCells = new List<CellData>();
}


[System.Serializable]
public class CellData
{
    public BoardCoordinate Coordinate;
    public GameObject CellObject;
}
