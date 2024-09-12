using UnityEngine;

namespace Enemy.EnemySkeleton
{
    public class SkeletonAttackState : EnemyState
    {
        private Enemy_Skeleton m_EnemySkeleton;

        
        public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, Enemy_Skeleton _skeleton)
            : base(_enemyBase, _enemyStateMachine, _animHash)
        {
            m_EnemySkeleton = _skeleton;
        }

        public override void Update()
        {
            base.Update();
            m_EnemySkeleton.SetZeroVelocity();

            
            if (m_TriggerCalled)
            {
                m_enemyStateMachine.ChangeState(m_EnemySkeleton.m_BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            m_EnemySkeleton.m_PreviousAttackTime = Time.time;
        }
    }
}
