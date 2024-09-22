using Misc;
using UnityEngine;

namespace Player.Skills.Crystal
{
    public class CrystalSkill : Skill
    {
        [SerializeField] private GameObject m_CrystalPrefab;
        [SerializeField] private float m_CrystalDuration;
        [SerializeField] private float m_TargetCheckRadius;


        private GameObject m_CurrentCrystal;

        [Header("Explosive Crystal")] 
        [SerializeField] private bool m_CanExplode;

        [Header("Moving Crystal")] 
        [SerializeField] private bool m_CanMoveTowardsEnemy;
        [SerializeField] private float m_MoveSpeed;
        
        public override void UseSkill()
        {
            base.UseSkill();
            if (m_CurrentCrystal == null)
            {
                m_CurrentCrystal = Instantiate(m_CrystalPrefab, m_Player.transform.position, Quaternion.identity);
                CrystalSkillController controller = m_CurrentCrystal.GetComponent<CrystalSkillController>();
                controller.SetupCrystal(m_CrystalDuration, m_CanExplode, m_CanMoveTowardsEnemy, m_MoveSpeed,
                    FindClosetEnemy(m_CurrentCrystal.transform, m_TargetCheckRadius));
            }
            // else
            // {
            //     m_Player.transform.position = m_CurrentCrystal.transform.position;
            //     CrystalSkillController controller = m_CurrentCrystal.GetComponent<CrystalSkillController>();
            //     controller.CrystalDestroyLogic();
            // }
        }
    }
}
