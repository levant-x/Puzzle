using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event System.Action<TableCell> onCellEnter;
    public static event System.Action<TableCell> onTileChange;
    public static TableCell current
    {
        get { return currentVar; }
        private set
        {
            currentVar = value;
            onCellEnter?.Invoke(currentVar);
        }
    }
    public bool isOccupied
    {
        get { return contentAttached != null; }
    }
    public bool isTileSetCorrectly
    {
        get { return contentAttached?.index == tileIndex; }
    }
    static int totalCount = 0;
    int tileIndex;
    static TableCell currentVar;
    IIndexedByNum contentAttached;

    
    void Start()
    {
        tileIndex = totalCount;
        totalCount++;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isOccupied) current = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        current = null;
    }

    public bool AttachTile(IIndexedByNum tile)
    {
        if (isOccupied) return false;
        contentAttached = tile;
        onTileChange?.Invoke(this);
        return true;
    }

    public void DetachTile(IIndexedByNum tile)
    {
        if (!tile.Equals(contentAttached)) return;
        contentAttached = null;
        onTileChange?.Invoke(this);
    }

    public override string ToString()
    {
        var result = $"{base.ToString()} for tile #{tileIndex}";
        if (isOccupied) result = $"{result} (currently set with {contentAttached.index})";
        return result;
    }
}
