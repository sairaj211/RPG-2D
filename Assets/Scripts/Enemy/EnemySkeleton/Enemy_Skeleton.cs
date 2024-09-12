using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

namespace Enemy.EnemySkeleton
{
    public class Enemy_Skeleton : Enemy
    {

        #region States

        public SkeletonIdleState m_IdleState { get; private set; }
        public SkeletonMoveState m_MoveState { get; private set; }
        public SkeletonBattleState m_BattleState { get; private set; }
        public SkeletonAttackState m_AttackState { get; private set; }
        public SkeletonStunnedState m_StunnedState { get; private set; }
        

        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            m_IdleState = new SkeletonIdleState(this, m_EnemyStateMachine, EntityStatesAnimationHash.IDLE, this);
            m_MoveState = new SkeletonMoveState(this, m_EnemyStateMachine, EntityStatesAnimationHash.MOVE, this);
            m_BattleState = new SkeletonBattleState(this, m_EnemyStateMachine, EntityStatesAnimationHash.MOVE, this);
            m_AttackState = new SkeletonAttackState(this, m_EnemyStateMachine, EntityStatesAnimationHash.ATTACK, this);
            m_StunnedState = new SkeletonStunnedState(this, m_EnemyStateMachine, EntityStatesAnimationHash.STUNNED, this);
        }

        protected override void Start()
        {
            base.Start();
            m_EnemyStateMachine.Initialize(m_IdleState);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.I))
            {
                m_EnemyStateMachine.ChangeState(m_StunnedState);
            }
        }

        public override bool CanBeStunned()
        {
            if (base.CanBeStunned())
            {
                m_EnemyStateMachine.ChangeState(m_StunnedState);
                return true;
            }

            return false;
        }
    }
}
