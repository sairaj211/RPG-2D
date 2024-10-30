using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash)
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Update()
    {
        base.Update();

        if (m_Player.IsWallDetected())
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_WallSlideState);
        }
        
        if (m_Player.IsGrounded())
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }

        // Falling down 
        if (m_InputMovement.x != 0f)
        {
              m_Player.SetVelocity(m_InputMovement.x * m_Player.m_MoveSpeed * m_Player.m_FallSpeed, m_Rigidbody2D.velocityY); 
        }
    }
}
