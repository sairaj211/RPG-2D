using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<InventoryItem> m_InventoryItemList;
    public Dictionary<ItemDataSO, InventoryItem> m_InventoryItemsDictionary;

    [Header("Inventory UI")] 
    [SerializeField] private Transform m_InventorySlotParentTransform;

    private UIItemSlot[] m_UIItemSlots;
    
    private void Start()
    {       
        m_InventoryItemList = new List<InventoryItem>(10);
        m_InventoryItemsDictionary = new Dictionary<ItemDataSO, InventoryItem>(5);

        m_UIItemSlots = m_InventorySlotParentTransform.GetComponentsInChildren<UIItemSlot>();
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < m_InventoryItemList.Count; ++i)
        {
            m_UIItemSlots[i].UpdateSlot(m_InventoryItemList[i]);
        }
    }

    public void AddItem(ItemDataSO _itemData)
    {
        if (m_InventoryItemsDictionary.TryGetValue(_itemData, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_itemData);
            m_InventoryItemList.Add(newItem);
            m_InventoryItemsDictionary.Add(_itemData, newItem);
        }
        
        UpdateSlotUI();
    }

    public void RemoveItem(ItemDataSO _itemData)
    {
        if (m_InventoryItemsDictionary.TryGetValue(_itemData, out InventoryItem inventoryItem))
        {
            if (inventoryItem.m_StackSize <= 1)
            {
                m_InventoryItemList.Remove(inventoryItem);
                m_InventoryItemsDictionary.Remove(_itemData);
            }
        }
        else
        {
            inventoryItem.RemoveStack();
        }
        
        UpdateSlotUI();
    }
}
