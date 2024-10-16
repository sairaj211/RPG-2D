using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")] 
    public Stat m_Strength;         // 1 point increases damage by 1 and crit power by 1%
    public Stat m_Agility;          // 1 point increases evasion by 1% and crit chance by 1%
    public Stat m_Intelligence;     // 1 point increases magic damage by 1 and magic resistance by 3
    public Stat m_Vitality;         // 1 point increases health by 3 or 5 points
    
    [Header("Offensive Stats")]
    public Stat m_Damage;
    public Stat m_CritPower;            // more the power more chances to crit  // default value is 150%
    public Stat m_CritChance;
    
    [Header("Defensive Stats")]
    public Stat m_MaxHealth;
    public Stat m_Armor;
    public Stat m_Evasion;
    public Stat m_MagicResistance;
    
    [Header("Magic Stats")] 
    public Stat m_FireDamage;
    public Stat m_IceDamage;
    public Stat m_ThunderDamage;

    public bool m_IsIgnited;
    public bool m_IsFrozen;
    public bool m_IsShocked;
    
    private int m_CurrentHealth;

    private Entity m_Entity;



    protected virtual void Start()
    {
        m_Entity = GetComponent<Entity>();
        m_CurrentHealth = m_MaxHealth.Value;
        m_CritPower.SetDefaultValue(150);
    }

    public void CalculateDamage(CharacterStats _targetStats)
    {
        int basicStatDamage = CalculateBaseStatsDamage(_targetStats);
        int magicalDamage = CalculateMagicalDamage(_targetStats);
        int totalDamage = basicStatDamage + magicalDamage;
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual int CalculateBaseStatsDamage(CharacterStats _targetStats)
    {
        if (CanEvade(_targetStats)) return 0;
        
        int totalDamage = m_Damage.Value + m_Strength.Value;

        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }
        
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        return totalDamage;
    }

    public virtual int CalculateMagicalDamage(CharacterStats _targetStats)
    {
        int fireDamage = m_FireDamage.Value;
        int iceDamage = m_IceDamage.Value;
        int thunderDamage = m_ThunderDamage.Value;

        int totalMagicalDamage  = fireDamage + iceDamage + thunderDamage + m_Intelligence.Value;
        totalMagicalDamage  = CheckTargetMagicResistance(_targetStats, totalMagicalDamage );

      
        
        // Check if all damage values are zero
        if (totalMagicalDamage  <= 0)
        {
            return 0;
        }
        
        // Determine which effects can be applied
        bool canApplyIgnite = fireDamage > Mathf.Max(iceDamage, thunderDamage);
        bool canApplyChill = iceDamage > Mathf.Max(fireDamage, thunderDamage);
        bool canApplyShock = thunderDamage > Mathf.Max(fireDamage, iceDamage);

        // Attempt to apply effects if none are currently valid
        if (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            ApplyRandomEffect(fireDamage, iceDamage, thunderDamage, _targetStats);
        }
        else
        {
            _targetStats.ApplyEffect(canApplyIgnite, canApplyChill, canApplyShock);
        }

        
        return totalMagicalDamage ;
    }

    private void ApplyEffect(bool _canApplyIgnite, bool _canApplyChill, bool _canApplyShock)
    {
        if(m_IsIgnited || m_IsFrozen || m_IsShocked) return;

        m_IsIgnited = _canApplyIgnite;
        m_IsFrozen = _canApplyChill;
        m_IsShocked = _canApplyShock;
    }
    
    private void ApplyRandomEffect(int fireDamage, int iceDamage, int thunderDamage, CharacterStats _targetStats)
    {
        if (Mathf.Max(fireDamage, iceDamage, thunderDamage) <= 0)
        {
            return;
        }
        
        float randValue = Random.value;

        if (randValue < 0.33f && fireDamage > 0)
        {
            Debug.Log("Apply Ignite");
            _targetStats.ApplyEffect(true, false, false);
        }
        else if (randValue < 0.66f && iceDamage > 0)
        {
            Debug.Log("Apply Chill");
            _targetStats.ApplyEffect(false, true, false);
        }
        else if (thunderDamage > 0)
        {
            Debug.Log("Apply Shock");
            _targetStats.ApplyEffect(false, false, true);
        }
    }

    private static int CheckTargetMagicResistance(CharacterStats _targetStats, int totalMagicalDamge)
    {

        totalMagicalDamge -= _targetStats.m_MagicResistance.Value + (_targetStats.m_Intelligence.Value * 3);
        totalMagicalDamge = Mathf.Max(totalMagicalDamge, 0);

        return totalMagicalDamge;
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

    private bool CanCrit()
    {
        int totalCritChance = m_CritChance.Value;

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (m_CritPower.Value + m_Strength.Value) * 0.01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        Debug.Log("Take damage");
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
