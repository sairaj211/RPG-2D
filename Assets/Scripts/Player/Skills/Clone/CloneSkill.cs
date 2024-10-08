using System.Collections;
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

        [SerializeField] private bool m_CreateCloneOnDashStart;
        [SerializeField] private bool m_CreateCloneOnDashEnd;
        [SerializeField] private bool m_CreateCloneOnCounterAttack;
        [SerializeField] private float m_DelayTime;
        [SerializeField] private float m_CloneSpawnOffset = 1f;
        [SerializeField] private bool m_CanDuplicateClone;
        [SerializeField] private int m_CloneDuplicateChances;

        public void CreateClone(Transform _clonePosition, Vector3 _offset = default)
        {
            GameObject clone = Instantiate(m_ClonePrefab);
            
            clone.GetComponent<CloneSkillController>().SetUpClone(_clonePosition, m_CloneDuration, m_CanAttack, m_CanDuplicateClone,m_CloneDuplicateChances, FindClosetEnemy(clone.transform , m_TargetCheckRadius),_offset);
        }

        public void CreateCloneOnDashStart()
        {
            TryCreateCloneAt(m_CreateCloneOnDashStart);
        }
        
        public void CreateCloneOnDashEnd()
        {
            TryCreateCloneAt(m_CreateCloneOnDashEnd);
        }

        private void TryCreateCloneAt(bool _canCreate)
        {
            if (_canCreate)
            {
                CreateClone(m_Player.transform, Vector3.zero);
            }
        }
        
        public void CreateCloneOnCounterAttack(Transform _enemyTransform)
        {
            if (m_CreateCloneOnCounterAttack)
            {
                StartCoroutine(CreateAfterDelay(_enemyTransform, new Vector3(m_CloneSpawnOffset * m_Player.m_FacingDireciton, 0f)));
            }
        }

        private IEnumerator CreateAfterDelay(Transform _transform, Vector3 _offset)
        {
            yield return new WaitForSeconds(m_DelayTime);
            CreateClone(_transform, _offset);
        }
    }
}
