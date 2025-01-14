using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "SO/Equipment")]
public class ItemDataEquipmentSO : ItemDataSO
{
    public EquipmentType m_EquipmentType;
}

