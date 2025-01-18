using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemDataSO m_ItemDataSO;

    private SpriteRenderer m_SpriteRenderer;

    private void OnValidate()
    {
        if (m_ItemDataSO != null)
        {
            if (m_ItemDataSO.m_Icon == null)
            {
                Debug.LogWarning("Item icon is not set.");
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = m_ItemDataSO.m_Icon;
                gameObject.name = m_ItemDataSO.m_ItemName;
            }
        }
    }

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = m_ItemDataSO.m_Icon;
    }

    private void OnTriggerEnter2D(Collider2D _collider2D)
    {
        if (_collider2D.TryGetComponent(out Player.Player player))
        {
            if (m_ItemDataSO.m_ItemType == ItemType.Material)
            {
                InventoryManager.Instance.AddItem(m_ItemDataSO);
            }
            else if (m_ItemDataSO.m_ItemType == ItemType.Equipment)
            {
                StashManager.Instance.AddItem(m_ItemDataSO);
            }

            Destroy(gameObject);
        }
    }
}
