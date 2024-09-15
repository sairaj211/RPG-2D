using Misc;
using UnityEngine;

namespace Player.Skills
{
    public class Clone_Skill : Skill
    {
        [Header("Clone Info")] 
        [SerializeField] private GameObject m_ClonePrefab;

        [SerializeField] private float m_CloneDuration;
        [SerializeField] private bool m_CanAttack;
        public void CreateClone(Transform _clonePosition)
        {
            GameObject clone = Instantiate(m_ClonePrefab);
            
            clone.GetComponent<Clone_SkillController>().SetUpClone(_clonePosition, m_CloneDuration, m_CanAttack);
        }
    }
}
