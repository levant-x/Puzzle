using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    [SerializeField] GameObject framePrefab = null;
    GameObject tableOriginObj, serveryOriginObj;
    Sprite[] tiles;
    int[,] gameField = new int[5, 5];
    Vector3 cellSize;
    float serveryLeft, serveryRight, serveryTop, serveryBottom;
    List<TableCell> putTogether = new List<TableCell>();


    void Start()
    {
        tableOriginObj = GameObject.Find("TableOrigin").gameObject;
        serveryOriginObj = GameObject.Find("ServeryOrigin").gameObject;
        tiles = Resources.LoadAll<Sprite>("Sprites/Tiles");
        CalcServeryBounds();

        cellSize = framePrefab.GetComponent<SpriteRenderer>().bounds.size;
        for (int y = gameField.GetLength(0) - 1; y >= 0; y--) GenerateTableRow(y);
        foreach (var tile in tiles) GenerateTile(tile);
        TableCell.onTileChanged += TableCell_OnTileChanged;
    }

    private void TableCell_OnTileChanged(TableCell cell)
    {
        if (cell.isTileSetCorrectly) putTogether.Add(cell);
        else putTogether.Remove(cell);
        if (putTogether.Count == gameField.Length) throw new System.Exception("Gameover");
    }

    void CalcServeryBounds()
    {
        var originRectT = serveryOriginObj.transform as RectTransform;
        var originPos = originRectT.position;
        serveryLeft = originPos.x - originRectT.sizeDelta.x / 2;
        serveryRight = originPos.x + originRectT.sizeDelta.x / 2;
        serveryTop = originPos.y + originRectT.sizeDelta.y / 2;
        serveryBottom = originPos.y - originRectT.sizeDelta.y / 2;
    }

    void GenerateTableRow(int y)
    {
        for (int x = 0; x < gameField.GetLength(1); x++) GenerateTableCell(x, y);
    }

    void GenerateTableCell(int x, int y)
    {
        var cellPos = tableOriginObj.transform.position +
            new Vector3(x * cellSize.x, y * cellSize.y);
        var cell = Instantiate(framePrefab, cellPos, Quaternion.identity);
        cell.transform.SetParent(tableOriginObj.transform);
    }

    void GenerateTile(Sprite tileSprite)
    {        
        var tile = new GameObject(tileSprite.name);
        tile.AddComponent<Tile>(); // controlling script
        tile.AddComponent<GraphicRaycaster>(); // to receive rays
        tile.AddComponent<Image>().sprite = tileSprite; // to show sprite
        var canvas = tile.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 1; 
        tile.transform.position = new Vector3(Random.Range(serveryLeft, serveryRight),
            Random.Range(serveryBottom, serveryTop));
        (tile.transform as RectTransform).sizeDelta = cellSize;    
    }
}
