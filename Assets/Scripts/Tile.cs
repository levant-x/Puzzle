using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public int index { get; private set; }
    Canvas canvas;
    Image image;
    TableCell enteredCell, homeCell;
    float cellBindingDist = 0.15f;


    void Start()
    {
        var indexStr = name.Replace("Tiles_", null);
        index = int.Parse(indexStr);
        TableCell.onCellEnter += TableCell_OnCellEnter;
        canvas = GetComponent<Canvas>();
        image = GetComponent<Image>();
    }

    public void OnDrag(PointerEventData data)
    {
        var pointerWorldPos = Camera.main.ScreenToWorldPoint(data.position);
        pointerWorldPos.z = 0;
        if (CanBindToCell(pointerWorldPos)) SetPosition(enteredCell.transform.position);
        else SetPosition(pointerWorldPos);
    }

    bool CanBindToCell(Vector3 pointerPos)
    {
        if (enteredCell == null || enteredCell.isOccupied) return false;
        var asdf = enteredCell.transform.parent.TransformPoint(pointerPos);
        var tileToCellDist = (enteredCell.transform.position - pointerPos).magnitude;  
        return tileToCellDist < cellBindingDist;
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
        if (CanBindToCell(transform.position) && enteredCell != null && enteredCell.AttachTile(this))
        {
            homeCell = enteredCell;
            canvas.sortingOrder = 1;
        }
        image.raycastTarget = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        homeCell?.DetachTile(this);
        canvas.sortingOrder = 2;
        image.raycastTarget = false;
    }
}
