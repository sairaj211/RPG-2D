using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private InventoryComponent m_InventoryComponent;
    [SerializeField] private Transform m_InventorySlotParent;

    private void Start()
    {
        // Initialize the inventory component with its UI parent
        m_InventoryComponent.Initialize(m_InventorySlotParent);
    }

    public void AddItem(ItemDataSO itemData)
    {
        m_InventoryComponent.AddItem(itemData);
    }

    public void RemoveItem(ItemDataSO itemData)
    {
        m_InventoryComponent.RemoveItem(itemData);
    }
    
    // public Dictionary<ItemDataSO, InventoryItem> m_InventoryItemsDictionary;
    // public Dictionary<ItemDataSO, InventoryItem> m_StashItemsDictionary;
    //
    // public List<InventoryItem> m_InventoryItemList;
    // public List<InventoryItem> m_StashItemList;
    //
    //
    // [Header("Inventory UI")] 
    // [SerializeField] private Transform m_InventorySlotParentTransform;
    // [SerializeField] private Transform m_StashSlotParentTransform;
    //
    // private UIItemSlot[] m_UIInventoryItemSlots;
    // private UIItemSlot[] m_UIStashItemSlots;
    //
    // private void Start()
    // {       
    //     m_InventoryItemList = new List<InventoryItem>(10);
    //     m_InventoryItemsDictionary = new Dictionary<ItemDataSO, InventoryItem>(5);
    //     
    //     m_StashItemList = new List<InventoryItem>(10);
    //     m_StashItemsDictionary = new Dictionary<ItemDataSO, InventoryItem>(5);
    //
    //     m_UIInventoryItemSlots = m_InventorySlotParentTransform.GetComponentsInChildren<UIItemSlot>();
    //     m_UIStashItemSlots = m_StashSlotParentTransform.GetComponentsInChildren<UIItemSlot>();
    // }
    //
    // private void UpdateSlotUI()
    // {
    //     for (int i = 0; i < m_InventoryItemList.Count; ++i)
    //     {
    //         m_UIInventoryItemSlots[i].UpdateSlot(m_InventoryItemList[i]);
    //     }
    //     
    //     for (int i = 0; i < m_StashItemList.Count; ++i)
    //     {
    //         m_UIStashItemSlots[i].UpdateSlot(m_StashItemList[i]);
    //     }
    // }
    //
    // public void AddItem(ItemDataSO _itemData)
    // {
    //     if (_itemData.m_ItemType == ItemType.Material)
    //     {
    //         if (m_InventoryItemsDictionary.TryGetValue(_itemData, out InventoryItem inventoryItem))
    //         {
    //             inventoryItem.AddStack();
    //         }
    //         else
    //         {
    //             InventoryItem newItem = new InventoryItem(_itemData);
    //             m_InventoryItemList.Add(newItem);
    //             m_InventoryItemsDictionary.Add(_itemData, newItem);
    //         }
    //     }
    //     else if (_itemData.m_ItemType == ItemType.Material)
    //     {
    //         if (m_StashItemsDictionary.TryGetValue(_itemData, out InventoryItem inventoryItem1))
    //         {
    //             inventoryItem1.AddStack();
    //         }
    //         else
    //         {
    //             InventoryItem newItem = new InventoryItem(_itemData);
    //             m_StashItemList.Add(newItem);
    //             m_StashItemsDictionary.Add(_itemData, newItem);
    //         }
    //     }
    //
    //     UpdateSlotUI();
    // }
    //
    // public void RemoveItem(ItemDataSO _itemData)
    // {
    //     if (m_InventoryItemsDictionary.TryGetValue(_itemData, out InventoryItem inventoryItem))
    //     {
    //         if (inventoryItem.m_StackSize <= 1)
    //         {
    //             m_InventoryItemList.Remove(inventoryItem);
    //             m_InventoryItemsDictionary.Remove(_itemData);
    //         }
    //     }
    //     else
    //     {
    //         inventoryItem.RemoveStack();
    //     }
    //     
    //     if (m_StashItemsDictionary.TryGetValue(_itemData, out InventoryItem inventoryItem1))
    //     {
    //         if (inventoryItem1.m_StackSize <= 1)
    //         {
    //             m_InventoryItemList.Remove(inventoryItem1);
    //             m_StashItemsDictionary.Remove(_itemData);
    //         }
    //     }
    //     else
    //     {
    //         inventoryItem1.RemoveStack();
    //     }
    //
    //     
    //     UpdateSlotUI();
    // }
}
