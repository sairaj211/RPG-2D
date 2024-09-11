using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _playerStateMachine, int _animHash)
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_PrimaryAttackState);
        }
        
        if (!m_Player.IsGrounded())
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_AirState);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && m_Player.IsGrounded())
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_JumpState);
        }
    }
}
