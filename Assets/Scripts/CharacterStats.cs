using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public Stat m_Strength;
    public Stat m_MaxHealth;
    public Stat m_Damage;

    private int m_CurrentHealth;

    private Entity m_Entity;
    
    protected virtual void Start()
    {
        m_Entity = GetComponent<Entity>();
        m_CurrentHealth = m_MaxHealth.Value;
    }

    public virtual void CalculateAndDoDamage(CharacterStats _targetStats)
    {



        int totalDamage = m_Damage.Value + m_Strength.Value;
        
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        Debug.Log("take damage");
        m_CurrentHealth -= _damage;

        if (m_CurrentHealth <= 0)
        {
            Die();
            return;
        }
        
        DamageEffect();
    }

    private void Die()
    {
        m_Entity.Death();
    }

    private void DamageEffect()
    {
        m_Entity.DamageEffect();
    }
}
