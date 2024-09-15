using Misc;
using UnityEngine;

namespace Player.Skills
{
    public class Sword_Skill : Skill
    {
        [Header("Skill Info")] 
        [SerializeField] private GameObject m_SwordPrefab;

        [SerializeField] private Vector2 m_LaunchDirection;
        [SerializeField] private float m_SwordGravity;


        public void CreateSword()
        {
            GameObject m_Sword = Instantiate(m_SwordPrefab, m_Player.transform.position, transform.rotation);
            Sword_SkillController m_SwordSkillController = m_Sword.GetComponent<Sword_SkillController>();
            
            m_SwordSkillController.SetUpSword(m_LaunchDirection, m_SwordGravity);
        }
    }
}
