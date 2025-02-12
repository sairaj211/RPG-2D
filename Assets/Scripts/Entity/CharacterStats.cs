using System;
using Misc;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public struct DamageType
{
    public bool m_IsPhysical;
    public bool m_IsMagical;

    public DamageType(bool _isPhysical, bool _isMagical)
    {
        m_IsPhysical = _isPhysical;
        m_IsMagical = _isMagical;
    }
}

public class CharacterStats : MonoBehaviour
{
    public Action OnHealthChangeEvent;
    public Action OnDeathEvent;

    private EntityFX m_EntityFX;
    
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

    public bool m_IsIgnited;    // take damage over time 
    public bool m_IsFrozen;     // freeze for certain duration  or can decrease armour by 20%
    public bool m_IsShocked;    // reduce accuracy by 20%
    
    private int m_CurrentHealth;

    private Entity m_Entity;
    
    public float m_MaxIgniteDuration;
    public float m_MaxFrozenDuration;
    public float m_MaxShockDuration;

    private float m_IgnitedTimer;
    private readonly float m_IgniteDamageCooldown = 0.3f;
    private float m_IgniteDamageTimer = Mathf.Infinity;
    private int m_IgniteDamage;
    private int m_ShockDamage;
    

    private float m_FrozenTimer;
    private float m_ShockedTimer;
    private bool m_HasDied;

    [SerializeField] private GameObject m_ThunderPrefab;
    [SerializeField]private float m_SlowPercentage = 0.2f;

    private bool m_IsPlayer = false;
    
    private void Awake()
    {
       m_CurrentHealth = GetMaxHealthValue();

       Player.Player player = GetComponent<Player.Player>();
       if (player == null)
       {
           m_IsPlayer = true;
       }
    }

    protected virtual void Start()
    {
        m_Entity = GetComponent<Entity>();
        m_EntityFX = GetComponent<EntityFX>();
        m_CritPower.SetDefaultValue(150);
        m_HasDied = false;
    }

    protected virtual void Update()
    {
        UpdateTimers();
        HandleIgniteDamage();
    }
    
    private void UpdateTimers()
    {
        m_IgnitedTimer -= Time.deltaTime;
        m_FrozenTimer -= Time.deltaTime;
        m_ShockedTimer -= Time.deltaTime;
        m_IgniteDamageTimer -= Time.deltaTime;

        m_IsIgnited = m_IgnitedTimer >= 0f;
        m_IsFrozen = m_FrozenTimer >= 0f;
        m_IsShocked = m_ShockedTimer >= 0f;
    }
    
    private void HandleIgniteDamage()
    {
        if (m_IgniteDamageTimer < 0f && m_IsIgnited)
        {
            Debug.Log("Take Ignite damage");

            DecreaseHealthBy(m_IgniteDamage);
            m_IgniteDamageTimer = m_IgniteDamageCooldown;
        }
    }

    public void CalculateAndApplyDamage(CharacterStats _targetStats, DamageType _damageType)
    {
        int totalDamage = 0;
        int physicalDamage = 0;
        int magicalDamage = 0;
        
        if (_damageType.m_IsPhysical)
        {
            physicalDamage = CalculatePhysicalDamage(_targetStats);
        }

        if (_damageType.m_IsMagical)
        {
            magicalDamage = CalculateMagicalDamage(_targetStats);
        }
        
        Debug.Log("PD" + physicalDamage);
        Debug.Log("MD" + magicalDamage);
        
        totalDamage = physicalDamage + magicalDamage;
        
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual int CalculatePhysicalDamage(CharacterStats _targetStats)
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
            _targetStats.ApplyEffect(canApplyIgnite, canApplyChill, canApplyShock, this);
        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * 0.05f));
        }
        
        if (canApplyShock)
        {
            _targetStats.SetupShockDamage(Mathf.RoundToInt(thunderDamage * 0.1f));
        }
        
        return totalMagicalDamage ;
    }

    private void ApplyEffect(bool _canApplyIgnite, bool _canApplyChill, bool _canApplyShock, CharacterStats _AttackersStats)
    {
        bool canApplyIgnite = !m_IsIgnited && !m_IsFrozen && !m_IsShocked;
        bool canApplyChill = !m_IsIgnited && !m_IsFrozen && !m_IsShocked;
        bool canApplyShock = !m_IsIgnited && !m_IsFrozen;
        
        if (_canApplyIgnite && canApplyIgnite)
        {
            m_IgnitedTimer = _AttackersStats.m_MaxIgniteDuration;
            m_IgniteDamageTimer = m_IgniteDamageCooldown;
            m_IsIgnited = true;
            m_EntityFX.IgniteFx(m_IgnitedTimer);
        }
        if (_canApplyChill && canApplyChill)
        {
            m_FrozenTimer = _AttackersStats.m_MaxFrozenDuration;
            m_IsFrozen = true;
            m_Entity.ApplySlowEffect(_AttackersStats.m_SlowPercentage, m_FrozenTimer);
            m_EntityFX.ChillFx(m_FrozenTimer);
        }
        if (_canApplyShock && canApplyShock)
        {
            if (!m_IsShocked)
            {
                m_ShockedTimer = _AttackersStats.m_MaxShockDuration;
                ApplyShockWithoutTimer();
            }
            else
            {
                if(!m_IsPlayer)
                    return;

                HitNearestEnemy();
            }
        }
    }

    public void ApplyShockWithoutTimer()
    {
        m_IsShocked = true;
        m_EntityFX.ShockFx(m_ShockedTimer);
    }

    private void HitNearestEnemy()
    {

        Collider2D[] m_Collider2D = Physics2D.OverlapCircleAll(transform.position, 25f);

        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;

        foreach (Collider2D hit in m_Collider2D)
        {
            if (hit.TryGetComponent(out Enemy.Enemy m_Enemy) &&
                Vector2.Distance(transform.position, hit.transform.position) > 1f)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                {
                    if (distanceToEnemy <= closestDistance)
                    {
                        closestEnemy = hit.transform;
                        closestDistance = distanceToEnemy;
                    }
                }
            }

            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }

        if (closestEnemy != null)
        {
            GameObject shockStrike = Instantiate(m_ThunderPrefab, transform.position, quaternion.identity);
            shockStrike.GetComponent<ThunderStrikeController>()
                .Setup(m_ShockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
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
            _targetStats.ApplyEffect(true, false, false, this);
        }
        else if (randValue < 0.66f && iceDamage > 0)
        {
            Debug.Log("Apply Chill");
            _targetStats.ApplyEffect(false, true, false, this);
        }
        else if (thunderDamage > 0)
        {
            Debug.Log("Apply Shock");
            _targetStats.ApplyEffect(false, false, true, this);
        }
    }

    public void SetupIgniteDamage(int _damage) => m_IgniteDamage = _damage;
    public void SetupShockDamage(int _damage) => m_ShockDamage = _damage;

    private int CheckTargetMagicResistance(CharacterStats _targetStats, int totalMagicalDamge)
    {
        totalMagicalDamge -= _targetStats.m_MagicResistance.Value + (_targetStats.m_Intelligence.Value * 3);
        totalMagicalDamge = Mathf.Max(totalMagicalDamge, 0);

        return totalMagicalDamge;
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage)
    {
        if (m_IsFrozen)
        {
            _totalDamage -= Mathf.RoundToInt(_targetStats.m_Armor.Value * 0.8f);
        }
        else
        {
            _totalDamage -= _targetStats.m_Armor.Value;
        }
        
        _totalDamage = Mathf.Max(_totalDamage, 0);
        return _totalDamage;
    }

    private bool CanEvade(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.m_Evasion.Value + _targetStats.m_Agility.Value;

        if (m_IsShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Damage avoided");
            return true;
        }
        return false;
    }

    private bool CanCrit() => Random.Range(0, 100) <= m_CritChance.Value;

    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (m_CritPower.Value + m_Strength.Value) * 0.01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        if (m_HasDied) return;
        
        Debug.Log("Take damage");
        DecreaseHealthBy(_damage);
        
        DamageEffect();
    }

    protected void DecreaseHealthBy(int _damage)
    {
        m_CurrentHealth -= _damage;
    
        OnHealthChangeEvent?.Invoke();
        
        if (m_CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
      //  m_Entity.Death();
      m_HasDied = true;
      OnDeathEvent?.Invoke();
    }

    private void DamageEffect()
    {
        m_Entity.DamageEffectAndImpact();
    }

    public int GetMaxHealthValue()
    {
        return m_MaxHealth.Value + m_Vitality.Value * 3;
    }

    public int GetCurrentHealth()
    {
        return m_CurrentHealth;
    }
}
