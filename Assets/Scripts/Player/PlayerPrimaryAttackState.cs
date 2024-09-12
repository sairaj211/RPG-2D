using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int m_ComboCounter;
    private float m_LastTimeAttacked;
    private float m_ComboWindow = 2f;

    private const float m_StopDelayDuringAttack = 0.15f;
    private float m_AttackDirection;
    
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, int _animHash)
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        m_InputMovement.x = 0; // fix on attack direction

        if (m_ComboCounter > 2 || Time.time >= m_LastTimeAttacked + m_ComboWindow)
        {
            m_ComboCounter = 0;
        }
        
        m_Animator.SetInteger(EntityStatesAnimationHash.COMBO_COUNTER, m_ComboCounter);

        m_StateTimer = m_StopDelayDuringAttack; // 0.1f

        // set attack direction        
        m_AttackDirection= m_Player.m_FacingDireciton;
        if (m_InputMovement.x != 0f)
        {
            m_AttackDirection = m_InputMovement.x;
        }
        
        m_Player.SetVelocity(m_Player.m_AttackMovements[m_ComboCounter].x * m_AttackDirection,
                                m_Player.m_AttackMovements[m_ComboCounter].y);
    }

    public override void Update()
    {
        base.Update();

        if (m_StateTimer < 0f)
        {
            m_Player.SetZeroVelocity();
        }
        
        if (m_TriggerCalled)
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        m_Player.StartCoroutine(nameof(Player.BusyFor), m_StopDelayDuringAttack); // 0.15f
        
        m_ComboCounter++;
        m_LastTimeAttacked = Time.time;
     }
}
