using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    [SerializeField] GameObject framePrefab = null, tilePrefab = null;
    GameObject tableOriginObj, heapOriginObj;
    Sprite[] tiles;
    Vector3 cellSize;
    float heapAreaLeft, heapAreaRight, heapAreaTop, heapAreaBottom;
    int rowsCount = 5, colsCount = 5;
    List<TableCell> tilesPutTogether = new List<TableCell>();


    void Start()
    {
        tableOriginObj = GameObject.Find("TableOrigin").gameObject;
        heapOriginObj = GameObject.Find("HeapOrigin").gameObject;
        tiles = Resources.LoadAll<Sprite>("Sprites/Tiles");
        CalcServeryBounds();

        cellSize = framePrefab.GetComponent<SpriteRenderer>().bounds.size;
        for (int y = colsCount - 1; y >= 0; y--) GenerateTableRow(y);
        foreach (var tile in tiles) GenerateTile(tile);
        TableCell.onTileChange += TableCell_OnTileChanged;
    }

    void GenerateTableRow(int y)
    {
        for (int x = 0; x < colsCount; x++) GenerateTableCell(x, y);
    }

    void GenerateTableCell(int x, int y)
    {
        var cellPos = tableOriginObj.transform.position +
            new Vector3(x * cellSize.x, y * cellSize.y);
        var cell = Instantiate(framePrefab, cellPos, Quaternion.identity);
        cell.transform.SetParent(tableOriginObj.transform);
    }

    private void TableCell_OnTileChanged(TableCell cell)
    {
        if (cell.isTileSetCorrectly) tilesPutTogether.Add(cell);
        else tilesPutTogether.Remove(cell);
        if (tilesPutTogether.Count == rowsCount * colsCount) Debug.Log("Game over!");
    }

    void CalcServeryBounds()
    {
        var originRectT = heapOriginObj.transform as RectTransform;
        var originPos = originRectT.position;
        heapAreaLeft = originPos.x - originRectT.sizeDelta.x / 2;
        heapAreaRight = originPos.x + originRectT.sizeDelta.x / 2;
        heapAreaTop = originPos.y + originRectT.sizeDelta.y / 2;
        heapAreaBottom = originPos.y - originRectT.sizeDelta.y / 2;
    }

    void GenerateTile(Sprite tileSprite)
    {
        var tile = Instantiate(tilePrefab, heapOriginObj.transform);
        tile.name = tileSprite.name;
        var tileSprRnr = tile.GetComponent<SpriteRenderer>();
        tileSprRnr.sprite = tileSprite; // setting sprite
        tile.transform.position = new Vector3(Random.Range(heapAreaLeft, heapAreaRight),
            Random.Range(heapAreaBottom, heapAreaTop));
        var tileSize = tileSprRnr.size;
        var newScale = new Vector3(cellSize.x / tileSize.x, cellSize.y / tileSize.y);
        tile.transform.localScale = newScale;
    }
}
