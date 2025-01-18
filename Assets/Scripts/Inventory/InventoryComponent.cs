using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour, IItemSlots
{
    protected Dictionary<ItemDataSO, InventoryItem> m_ItemsDictionary;
    protected List<InventoryItem> m_ItemList;
    protected UIItemSlot[] m_UISlots;

    // Called to initialize the inventory component with its UI parent
    public void Initialize(Transform slotParent)
    {
        m_ItemList = new List<InventoryItem>(10);
        m_ItemsDictionary = new Dictionary<ItemDataSO, InventoryItem>(5);
        m_UISlots = slotParent.GetComponentsInChildren<UIItemSlot>();
    }

    public void AddItem(ItemDataSO itemData)
    {
        if (m_ItemsDictionary.TryGetValue(itemData, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            m_ItemList.Add(newItem);
            m_ItemsDictionary.Add(itemData, newItem);
        }
        UpdateSlotUI();
    }

    public void RemoveItem(ItemDataSO itemData)
    {
        if (m_ItemsDictionary.TryGetValue(itemData, out InventoryItem inventoryItem))
        {
            if (inventoryItem.m_StackSize <= 1)
            {
                m_ItemList.Remove(inventoryItem);
                m_ItemsDictionary.Remove(itemData);
            }
            else
            {
                inventoryItem.RemoveStack();
            }
        }
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < m_ItemList.Count; ++i)
        {
            m_UISlots[i].UpdateSlot(m_ItemList[i]);
        }
    }
}

