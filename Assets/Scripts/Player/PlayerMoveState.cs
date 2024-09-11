using System;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _playerStateMachine, int _animHash)
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Update()
    {
        base.Update();

        m_Player.SetVelocity(m_Movement.x * m_Player.m_MoveSpeed, base.m_Rigidbody2D.velocityY);

        if (m_Movement.x == 0 || m_Player.IsWallDetected())
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }
    }

}
