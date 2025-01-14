using System;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash)
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Update()
    {
        base.Update();

        m_Player.SetVelocity(m_InputMovement.x * m_Player.m_MoveSpeed, base.m_Rigidbody2D.linearVelocityY);

        //m_Player.HandleMovement(Mathf.MoveTowards(m_Rigidbody2D.velocityX, m_InputMovement.x * m_Player.m_MoveSpeed, Time.fixedDeltaTime));
        
        

        if (m_InputMovement.x == 0 || m_Player.IsWallDetected())
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }
    }

}
