﻿namespace Enemy.EnemySkeleton
{
    public class SkeletonGroundedState : EnemyState
    {
        protected readonly EnemySkeleton m_EnemySkeleton;

        protected SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, EnemySkeleton _skeleton) 
            : base(_enemyBase, _enemyStateMachine, _animHash)
        {
            m_EnemySkeleton = _skeleton;
        }

        public override void Update()
        {
            base.Update();

            if (m_EnemySkeleton.IsPlayerDetected())
            {
                m_enemyStateMachine.ChangeState(m_EnemySkeleton.m_BattleState);
            }
        }
    }
}
