using System;
using Player.Skills;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    public Dash_Skill m_DashSkill { get; private set; }
    public Clone_Skill m_CloneSkill { get; private set; }
    public Sword_Skill m_SwordSkill { get; private set; }

    private void Start()
    {
        m_DashSkill = GetComponent<Dash_Skill>();
        m_CloneSkill = GetComponent<Clone_Skill>();
        m_SwordSkill = GetComponent<Sword_Skill>();
    }
}
