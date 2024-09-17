using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash) 
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        m_Player.m_SkillManager.m_SwordSkill.ActivateDots(true);
    }

    public override void Update()
    {
        base.Update();
        
        m_Player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            m_Player.m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var m_Position = m_Player.transform.position;
        bool shouldFlip = (m_Position.x > mousePosition.x && m_Player.m_FacingDireciton == 1) ||
                          (m_Position.x < mousePosition.x && m_Player.m_FacingDireciton == -1);

        if (shouldFlip)
        {
            m_Player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();

        m_Player.StartCoroutine(m_Player.BusyFor(0.2f));
    }
}
