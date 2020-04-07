using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IBeginDragHandler, IEndDragHandler,  IDragHandler, IIndexedByNum
{
    public int index { get; private set; }
    SpriteRenderer sprRenderer;
    BoxCollider2D boxColl2d;
    TableCell enteredCell, dockCell;
    float cellBindingDist = 0.15f;


    void Start()
    {
        var indexStr = name.Replace("Tiles_", null);
        index = int.Parse(indexStr);
        TableCell.onCellEnter += TableCell_OnCellEnter;
        sprRenderer = GetComponent<SpriteRenderer>();
        boxColl2d = GetComponent<BoxCollider2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dockCell?.DetachTile(this);
        sprRenderer.sortingLayerName = "Tiles";
        sprRenderer.sortingOrder = 1;
        gameObject.layer = 2;
        transform.SetAsFirstSibling();
    }

    public void OnDrag(PointerEventData data)
    {
        var pointerWorldPos = ConvertPosition(data.position);
        if (CloseToCellCenter(pointerWorldPos)) SetPosition(enteredCell.transform.position);
        else SetPosition(pointerWorldPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        sprRenderer.sortingOrder = 0;
        TryAttachToCell(ConvertPosition(eventData.position));
        gameObject.layer = 0;
    }

    bool CloseToCellCenter(Vector3 pointerPos)
    {
        if (enteredCell == null) return false;
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

    void TryAttachToCell(Vector3 position)
    {
        bool isAttachedToCell = CloseToCellCenter(position) &&
            enteredCell != null && enteredCell.AttachTile(this);
        if (!isAttachedToCell) return;
        sprRenderer.sortingLayerName = "Default";
        sprRenderer.sortingOrder = 1;
        dockCell = enteredCell;
    }

    Vector3 ConvertPosition(Vector3 screenPos)
    {
        var result = Camera.main.ScreenToWorldPoint(screenPos);
        result.z = 0;
        return result;
    }
}
