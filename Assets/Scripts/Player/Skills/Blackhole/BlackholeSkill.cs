using Misc;
using Unity.Mathematics;
using UnityEngine;

namespace Player.Skills.Blackhole
{
    public class BlackholeSkill : Skill
    {
        [Header("Blackhole Info")]
        [SerializeField] private GameObject m_BlackholePrefab;
        [SerializeField] private float m_MaxSize;
        [SerializeField] private float m_GrowSpeed;   
        [SerializeField] private int m_NumberOfAttacks;
        [SerializeField] private float m_CloneAttackCooldown;
        [SerializeField] private float m_SkillDuration;

        private BlackholeSkillController m_BlackholeSkillController;
        public override void UseSkill()
        {
            base.UseSkill();

            GameObject blackhole = Instantiate(m_BlackholePrefab, m_Player.transform.position, quaternion.identity);

            m_BlackholeSkillController = blackhole.GetComponent<BlackholeSkillController>();
            
            m_BlackholeSkillController.SetupBlackhole(m_MaxSize, m_GrowSpeed, m_NumberOfAttacks, m_CloneAttackCooldown, m_SkillDuration);
        }

        public bool SkillCompleted()
        {
            if (!m_BlackholeSkillController) return false;
            
            if (m_BlackholeSkillController.m_PlayerCanExitState)
            {
                m_BlackholeSkillController = null;
                return true;
            }

            return false;
        }
    }
}
