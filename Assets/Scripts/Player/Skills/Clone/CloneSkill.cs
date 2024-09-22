using Misc;
using UnityEngine;

namespace Player.Skills
{
    public class CloneSkill : Skill
    {
        [Header("Clone Info")] 
        [SerializeField] private GameObject m_ClonePrefab;

        [SerializeField] private float m_CloneDuration;
        [SerializeField] private bool m_CanAttack;
        [SerializeField] private float m_TargetCheckRadius;
        public void CreateClone(Transform _clonePosition, Vector3 _offset = default)
        {
            GameObject clone = Instantiate(m_ClonePrefab);
            
            clone.GetComponent<CloneSkillController>().SetUpClone(_clonePosition, m_CloneDuration, m_CanAttack, FindClosetEnemy(clone.transform , m_TargetCheckRadius),_offset);
        }
    }
}
