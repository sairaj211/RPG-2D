using System;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash) 
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_WallJumpState);
            return;
        }
        
        // if (m_Movement.x != 0f && m_Player.m_FacingDireciton != m_Movement.x)
        // {
        //     m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        // }
        
        if (m_InputMovement.x != 0f && Math.Abs(m_Player.m_FacingDireciton - m_InputMovement.x) > 0.01f)
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }

        float moveDownSpeed = 0.7f;
        if (m_InputMovement.y < 0f)
        {
            moveDownSpeed = 1f;
        }
        m_Player.SetVelocity(0, m_Rigidbody2D.velocityY * moveDownSpeed);
        
        if (m_Player.IsGrounded())
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }

    }
}
