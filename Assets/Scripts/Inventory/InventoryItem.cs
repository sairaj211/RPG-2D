using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemDataSO m_ItemDataSO;
    public int m_StackSize = 0;

    public InventoryItem(ItemDataSO _itemData)
    {
        m_ItemDataSO = _itemData;
        AddStack();
    }

    public void AddStack() => m_StackSize++;
    public void RemoveStack() => m_StackSize--;
}
