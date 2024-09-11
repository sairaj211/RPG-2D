using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _playerStateMachine, int _animHash) 
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();

        base.m_Rigidbody2D.velocity = Vector2.up * m_Player.m_JumpVelocity;
    }

    public override void Update()
    {
        base.Update();
         
        if (m_Rigidbody2D.velocityY < 0)
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_AirState);
        }
    }
}
