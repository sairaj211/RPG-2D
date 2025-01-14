using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform m_Sword;
    
    public PlayerCatchSwordState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash) 
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();

        m_Sword = m_Player.m_Sword.transform;
        
        var m_Position = m_Player.transform.position;
        var m_Position1 = m_Sword.position;
        bool shouldFlip = (m_Position.x > m_Position1.x && m_Player.m_FacingDireciton == 1) ||
                          (m_Position.x < m_Position1.x && m_Player.m_FacingDireciton == -1);

        if (shouldFlip)
        {
            m_Player.Flip();
        }

        m_Rigidbody2D.linearVelocity = new Vector2(m_Player.m_SwordReturnImpact * -m_Player.m_FacingDireciton,
            m_Rigidbody2D.linearVelocityY);
    }

    public override void Update()
    {
        base.Update();

        if (m_TriggerCalled)
        {
            m_Player.m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }
    }
    
    public override void Exit()
    {
        base.Exit();

        m_Player.StartCoroutine(m_Player.BusyFor(0.15f));
    }
}
