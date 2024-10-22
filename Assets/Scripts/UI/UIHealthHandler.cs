using System;
using UnityEngine;
using UnityEngine.UI;


public class UIHealthHandler : MonoBehaviour
{
    private Entity m_Entity;
    private CharacterStats m_CharacterStats;
    private RectTransform m_RectTransform;
    private Slider m_Slider;

    private void Start()
    {
        m_Entity = GetComponentInParent<Entity>();
        m_CharacterStats = GetComponentInParent<CharacterStats>();
        m_RectTransform = GetComponent<RectTransform>();
        m_Slider = GetComponentInChildren<Slider>();
        
        m_Slider.maxValue = m_CharacterStats.GetMaxHealthValue();
        OnHealthChangeEvent();
            
        m_Entity.OnFlippedEvent += OnFlippedEvent;
        m_CharacterStats.OnHealthChangeEvent += OnHealthChangeEvent;
    }

    private void OnHealthChangeEvent()
    {
        m_Slider.value = m_CharacterStats.GetCurrentHealth();
    }


    private void OnDisable()
    {
        m_Entity.OnFlippedEvent -= OnFlippedEvent;
        m_CharacterStats.OnHealthChangeEvent -= OnHealthChangeEvent;
    }

    private void OnFlippedEvent()
    {
        m_RectTransform.Rotate(0,180,0);
    }
}
        

