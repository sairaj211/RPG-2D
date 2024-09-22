using UnityEngine;

namespace Misc
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] protected float m_Cooldown;
        protected float m_CooldownTimer;

        protected Player.Player m_Player;
        
        protected virtual void Start()
        {
            m_Player = PlayerManager.Instance.m_Player;
        }
        
        protected virtual void Update()
        {
            m_CooldownTimer -= Time.deltaTime;
        }

        public virtual bool CanUseSkill()
        {
            if (m_CooldownTimer <= 0f)
            {
                UseSkill();
                m_CooldownTimer = m_Cooldown;
                return true;
            }

            return false;
        }

        public virtual void UseSkill()
        {
            // do some skill specific things
        }

        protected virtual Transform FindClosetEnemy(Transform _transform, float _radius)
        {
            Collider2D[] m_Collider2D = Physics2D.OverlapCircleAll(_transform.position, _radius);

            float closestDistance = float.MaxValue;
            Transform closestEnemy = null;
            
            foreach (Collider2D hit in m_Collider2D)
            {
                if (hit.TryGetComponent(out Enemy.Enemy m_Enemy))
                {
                    float distanceToEnemy = Vector2.Distance(_transform.position, hit.transform.position);
                    {
                        if (distanceToEnemy <= closestDistance)
                        {
                            closestEnemy = hit.transform;
                            closestDistance = distanceToEnemy;
                        }
                    }
                }
            }

            return closestEnemy;
        }
    }
}
