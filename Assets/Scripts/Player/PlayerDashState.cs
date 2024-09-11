using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, int _animHash)
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        m_StateTimer = m_Player.m_DashDuration;
    }

    public override void Update()
    {
        base.Update();

        if (!m_Player.IsGrounded() && m_Player.IsWallDetected())
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_WallSlideState);
        }
        
        //m_Player.SetDashVelocity(true);
        m_Player.SetVelocity(m_Player.m_DashSpeed* m_Player.m_DashDirection, 0f);
        
        if (m_StateTimer < 0f)
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
     //   m_Player.SetDashVelocity(false);
        m_Player.SetVelocity(0f, m_Rigidbody2D.velocityY);
    }
}
