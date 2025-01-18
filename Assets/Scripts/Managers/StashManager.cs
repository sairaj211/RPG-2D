using UnityEngine;

public class StashManager : Singleton<StashManager>
{
    [SerializeField] private InventoryComponent m_StashComponent;
    [SerializeField] private Transform m_StashSlotParent;

    private void Start()
    {
        // Initialize the stash component with its UI parent
        m_StashComponent.Initialize(m_StashSlotParent);
    }

    // Add item to the stash
    public void AddItem(ItemDataSO itemData)
    {
        m_StashComponent.AddItem(itemData);
    }

    // Remove item from the stash
    public void RemoveItem(ItemDataSO itemData)
    {
        m_StashComponent.RemoveItem(itemData);
    }
}

