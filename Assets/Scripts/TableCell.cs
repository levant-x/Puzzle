using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableCell : MonoBehaviour, IPointerEnterHandler
{
    public static event System.Action<TableCell> onCellEnter;
    public static event System.Action<TableCell> onTileChanged;
    public bool isOccupied
    {
        get { return tileAttached != null; }
    }
    public bool isTileSetCorrectly
    {
        get { return tileAttached?.index == tileIndex; }
    }

    static int totalCount = 0;
    int tileIndex;
    Tile tileAttached;

    
    void Start()
    {
        tileIndex = totalCount;
        totalCount++;
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isOccupied && onCellEnter != null) onCellEnter(this);
    }

    public bool AttachTile(Tile tile)
    {
        if (tileAttached != null) return false;
        tileAttached = tile;
        tile.transform.SetAsFirstSibling();
        onTileChanged?.Invoke(this);
        return true;
    }

    public void DetachTile(Tile tile)
    {
        if (!tile.Equals(tileAttached)) return;
        tileAttached = null;
        onTileChanged?.Invoke(this);
    }

    public override string ToString()
    {
        var result = $"{base.ToString()} for tile #{tileIndex}";
        if (isOccupied) result = $"{result} (currently set with {tileAttached.index})";
        return result;
    }
}
