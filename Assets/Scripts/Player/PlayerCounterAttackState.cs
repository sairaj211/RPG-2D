
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool m_CanCreateClone;
    
    public PlayerCounterAttackState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash) 
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Enter()
    {
        base.Enter();
        m_CanCreateClone = true;
        m_StateTimer = m_Player.m_CounterAttackDuration;
        m_Player.GetAnimator().SetBool(EntityStatesAnimationHash.SUCCESSFULCOUNTERATTACK,false);
    }

    public override void Update()
    {
        base.Update();
        
        m_Player.SetZeroVelocity();
        
        Collider2D[] m_Collider2D = Physics2D.OverlapCircleAll(m_Player.m_AttackCheck.position, m_Player.m_AttackCheckRadius);

        foreach (Collider2D hit in m_Collider2D)
        {
            if (hit.TryGetComponent<Enemy.Enemy>(out var Enemy))
            {
                if (Enemy.CanBeStunned())
                {
                    m_StateTimer = 10f; // any value bigger than counterAttackDuration
                    m_Player.GetAnimator().SetBool(EntityStatesAnimationHash.SUCCESSFULCOUNTERATTACK,true);

                    if (m_CanCreateClone)
                    {
                        m_CanCreateClone = false;
                        m_Player.m_SkillManager.m_CloneSKill.CreateCloneOnCounterAttack(hit.transform);
                    }
                }
            }
        }

        if (m_StateTimer < 0f || m_TriggerCalled)
        {
            m_PlayerStateMachine.ChangeState(m_Player.m_IdleState);
        }
    }
}
