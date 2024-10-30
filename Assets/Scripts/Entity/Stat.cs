using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Stat
{
    [SerializeField] private int m_BaseValue;

    public List<int> m_Modifiers;

    public int Value
    {
        get
        {
            int finalVal = m_BaseValue;
            foreach (var modifier in m_Modifiers)
            {
                finalVal += modifier;
            }
            return finalVal;
        }
    }

    public void SetDefaultValue(int _value)
    {
        m_BaseValue = _value;
    }

    public void AddModifiers(int _modifier)
    {
        m_Modifiers.Add(_modifier);
    }
    
    public void RemoveModifiers(int _modifier)
    {
        m_Modifiers.Remove(_modifier);
    }
}
