using UnityEngine;

namespace Enemy.EnemySkeleton
{
    public class SkeletonAttackState : EnemyState
    {
        private Enemy_Skeleton m_Enemy;

        
        public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, Enemy_Skeleton _skeleton)
            : base(_enemyBase, _enemyStateMachine, _animHash)
        {
            m_Enemy = _skeleton;
        }

        public override void Update()
        {
            base.Update();
            m_Enemy.SetZeroVelocity();

            
            if (m_TriggerCalled)
            {
                m_enemyStateMachine.ChangeState(m_Enemy.m_BattleState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            m_Enemy.m_PreviousAttackTime = Time.time;
        }
    }
}
