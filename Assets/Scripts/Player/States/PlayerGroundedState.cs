using Player.Skills.Sword;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash)
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse1) && !m_Player.m_Sword)
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_PlayerAimSwordState);
        }

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
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_PlayerCounterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse2) && m_Player.m_Sword)
        {
            m_Player.m_Sword.GetComponent<SwordSkillController>().ReturnSword();
        }
        
        if (Input.GetKeyDown(KeyCode.R) )
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_PlayerBlackholeState);
        }
    }
}
