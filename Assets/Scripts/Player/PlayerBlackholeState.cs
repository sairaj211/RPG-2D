using UnityEngine;

namespace Player
{
    public class PlayerBlackholeState : PlayerState
    {
        private float m_FlyTime = 0.4f;
        private bool m_SkillUsed = false;
        private float m_DefaultGravity;
        public PlayerBlackholeState(Player _player, PlayerStateMachine _playerStateMachine, int _animHash) 
            : base(_player, _playerStateMachine, _animHash)
        {
        }

        public override void Enter()
        {
            base.Enter();

            m_SkillUsed = false;
            m_StateTimer = m_FlyTime;
            m_DefaultGravity = m_Rigidbody2D.gravityScale; 
            m_Rigidbody2D.gravityScale = 0f;
        }

        public override void Update()
        {
            base.Update();

            if (m_StateTimer > 0f)
            {
                m_Rigidbody2D.velocity = new Vector2(0, 15f);
            }

            if (m_StateTimer < 0f)
            {
                m_Rigidbody2D.velocity = new Vector2(0, -0.1f);

                if (!m_SkillUsed)
                {
                    if (m_Player.m_SkillManager.m_BlackholeSkill.CanUseSkill())
                    {
                        m_SkillUsed = true;
                    }
                }
            }

            if (m_Player.m_SkillManager.m_BlackholeSkill.SkillCompleted())
            {
                m_PlayerStateMachine.ChangeState(m_Player.m_AirState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            m_Player.MakeTransparent(false);

            m_Rigidbody2D.gravityScale = m_DefaultGravity;
        }

        ///////////////////////////////////////////////////////////////////////////////////
        /// We exit the blackhole state from BlackholeSkillController
        ///////////////////////////////////////////////////////////////////////////////////
    }
}
