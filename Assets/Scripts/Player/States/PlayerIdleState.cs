using System;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash)
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        m_Player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if ((Math.Abs(m_InputMovement.x - m_Player.m_FacingDireciton) < 0.01f) && m_Player.IsWallDetected())
        {
            return;
        }
        
        if (m_InputMovement.x != 0 && !m_Player.m_IsBusy)
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_MoveState);
        }
    }
}
