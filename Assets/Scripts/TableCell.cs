using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TableCell : MonoBehaviour, IPointerEnterHandler
{
    public static event System.Action<TableCell> onCellEnter;
    public bool isOccupied
    {
        get => tileAttached != null;
    }
    public bool isTileSetCorrectly
    {
        get => tileAttached?.index == tileIndex;
    }

    static int totalCount;
    int tileIndex;
    Tile tileAttached;


    public TableCell()
    {
        tileIndex = totalCount;
        totalCount++;
    }

    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"{name} entered");
        if (!isOccupied && onCellEnter != null) onCellEnter(this);
    }

    public bool AttachTile(Tile tile)
    {
        if (tileAttached != null) return false;
        tileAttached = tile;
        tile.transform.SetAsLastSibling();
        onCellEnter?.Invoke(this);

        Debug.LogError("attt" + this );
        return true;
    }

    public void DetachTile(Tile tile)
    {
        if (tile.Equals(tileAttached))
        {
            tileAttached = null;
            Debug.Log("detached" + this);
        }
    }

    public override string ToString()
    {
        var result = $"{base.ToString()} for tile #{tileIndex}";
        if (isOccupied) result = $"{result} (currently set with {tileAttached.index})";
        return result;
    }
}
