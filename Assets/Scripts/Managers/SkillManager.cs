using System;
using Player.Skills;
using Player.Skills.Blackhole;
using Player.Skills.Crystal;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    public DashSkill m_DashSkill { get; private set; }
    public CloneSkill m_CloneSKill { get; private set; }
    public SwordSkill m_SwordSkill { get; private set; }
    public BlackholeSkill m_BlackholeSkill { get; private set; }
    public CrystalSkill m_CrystalSkill { get; private set; }

    private void Start()
    {
        m_DashSkill = GetComponent<DashSkill>();
        m_CloneSKill = GetComponent<CloneSkill>();
        m_SwordSkill = GetComponent<SwordSkill>();
        m_BlackholeSkill = GetComponent<BlackholeSkill>();
        m_CrystalSkill = GetComponent<CrystalSkill>();
    }
}
