using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Enemy.EnemySkeleton
{
    public class SkeletonDeadState : EnemyState
    {
        protected readonly Enemy_Skeleton m_EnemySkeleton;
        private bool m_TriggerHit;

        public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, Enemy_Skeleton _skeleton) 
            : base(_enemyBase, _enemyStateMachine, _animHash)
        {
            m_EnemySkeleton = _skeleton;
        }

        public override void Enter()
        {
            base.Enter();
            m_TriggerHit = false;
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            m_TriggerHit = true;
        }

        public override void Update()
        {
            base.Update();
            if (m_TriggerHit)
            {
                m_enemyStateMachine.ChangeState(m_EnemySkeleton.m_IdleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            m_EnemySkeleton.DestroyGameObject();
        }
        
    }
}
