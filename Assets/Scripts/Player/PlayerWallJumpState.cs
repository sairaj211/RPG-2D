using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash) 
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();

        m_StateTimer = m_Player.m_WallJumpDuration;
        m_Player.SetVelocity(m_Player.m_WallJumpVelocity * -m_Player.m_FacingDireciton, m_Player.m_JumpVelocity);
    }

    public override void Update()
    {
        base.Update();
        if (m_StateTimer < 0f)
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_AirState);
        }

        if (m_Player.IsGrounded())
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
