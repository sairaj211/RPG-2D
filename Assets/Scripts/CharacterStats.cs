using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")] 
    public Stat m_Strength;         // 1 point increases damage by 1 and crit power by 1%
    public Stat m_Agility;          // 1 point increases evasion by 1% and crit chance by 1%
    public Stat m_Intelligence;     // 1 point increases magic damage by 1 and magic resistance by 3
    public Stat m_Vitality;         // 1 point increases health by 3 or 5 points
    
    [Header("Defensive Stats")]
    public Stat m_MaxHealth;
    public Stat m_Armor;
    public Stat m_Evasion;
    
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
        if (CanEvade(_targetStats)) return;
        
        int totalDamage = m_Damage.Value + m_Strength.Value;
        
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        
        _targetStats.TakeDamage(totalDamage);
    }

    private static int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage)
    {
        _totalDamage -= _targetStats.m_Armor.Value;
        _totalDamage = Mathf.Max(_totalDamage, 0);
        return _totalDamage;
    }

    private static bool CanEvade(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.m_Evasion.Value + _targetStats.m_Agility.Value;

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Damage avoided");
            return true;
        }
        return false;
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
