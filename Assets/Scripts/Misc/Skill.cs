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
    }
}
