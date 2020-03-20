using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public int index { get; private set; }
    Canvas canvas;
    TableCell enteredCell, homeCell;
    float cellBindingDist = 0.15f;


    void Start()
    {
        var indexStr = name.Replace("Tiles_", null);
        index = int.Parse(indexStr);
        TableCell.onCellEnter += TableCell_OnCellEnter;
        canvas = GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData data)
    {
        var pointerWorldPos = Camera.main.ScreenToWorldPoint(data.position);
        if (CanBindToCell(transform.position)) SetPosition(enteredCell.transform.position);
        else SetPosition(pointerWorldPos);
    }

    bool CanBindToCell(Vector3 pointerPos)
    {
        var result = enteredCell != null && !enteredCell.isOccupied &&
            (enteredCell.transform.position - pointerPos).magnitude < cellBindingDist;
        return result;
    }

    void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    private void TableCell_OnCellEnter(TableCell cell)
    {
        enteredCell = cell;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("tile up");
        Debug.Log(enteredCell);
        if (CanBindToCell(transform.position) && enteredCell != null &&
            enteredCell.AttachTile(this)) homeCell = enteredCell;
        canvas.sortingOrder = 2;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("tile down");
        homeCell?.DetachTile(this);
        canvas.sortingOrder = 0;
    }
}
