using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item", menuName = "SO/Item")]
public class ItemDataSO : ScriptableObject
{
    public ItemType m_ItemType;
    public string m_ItemName;
    public Sprite m_Icon;
}

