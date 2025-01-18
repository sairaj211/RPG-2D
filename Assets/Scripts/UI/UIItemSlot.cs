using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image m_ItemImage;
    [SerializeField] private TextMeshProUGUI m_ItemText;

    public InventoryItem m_InventoryItem;
    
    public void UpdateSlot(InventoryItem _newItem)
    {
        m_InventoryItem = _newItem;
        
        m_ItemImage.color = Color.white;
        
        if (m_InventoryItem != null)
        {
            m_ItemImage.sprite = m_InventoryItem.m_ItemDataSO.m_Icon;

            if (m_InventoryItem.m_StackSize > 1)
            {
                m_ItemText.text = m_InventoryItem.m_StackSize.ToString();
            }
            else
            {
                m_ItemText.text = "";
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_InventoryItem.m_ItemDataSO.m_ItemType == ItemType.Equipment)
        {
            Debug.Log(m_InventoryItem.m_ItemDataSO.m_ItemName);
        }
    }
}
