using Enemy;
using Enemy.EnemySkeleton;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton m_EnemySkeleton;
    
    public SkeletonStunnedState(Enemy.Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, EnemySkeleton _skeleton) 
        : base(_enemyBase, _enemyStateMachine, _animHash)
    {
        m_EnemySkeleton = _skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        m_StateTimer = m_EnemySkeleton.m_StunDuration;
        
        m_EnemySkeleton.SetVelocity(m_EnemySkeleton.m_FacingDireciton * m_EnemySkeleton.m_StunDirection.x, m_EnemySkeleton.m_StunDirection.y, false);
    }

    public override void Update()
    {
        base.Update();
        m_EnemySkeleton.m_EntityFX.BlinkEffect();
        if (m_StateTimer < 0f)
        {
            m_EnemySkeleton.m_EntityFX.ResetBlink();
            m_enemyStateMachine.ChangeState(m_EnemySkeleton.m_IdleState);
        }
    }
}
