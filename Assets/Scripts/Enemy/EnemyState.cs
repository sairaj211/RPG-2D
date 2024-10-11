using UnityEngine;

namespace Enemy
{
    public class EnemyState
    {
        protected EnemyStateMachine m_enemyStateMachine;
        protected Enemy m_EnemyBase;
        protected Rigidbody2D m_Rigidbody2D;
        protected Animator m_Animator;
        protected Transform m_Player;
        private int m_AnimationHash;
        protected float m_StateTimer;
        protected bool m_TriggerCalled;
        protected AnimatorStateInfo m_AnimatorStateInfo;

        public EnemyState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash)
        {
            m_EnemyBase = _enemyBase;
            m_enemyStateMachine = _enemyStateMachine;
            m_AnimationHash = _animHash;
        }
        
        public virtual void Enter()
        {
            m_Animator = m_EnemyBase.GetAnimator();
            m_Animator.SetBool(m_AnimationHash, true);
            m_AnimatorStateInfo = GetAnimatorStateInfo();
            m_Rigidbody2D = m_EnemyBase.GetRigidbody();
            m_TriggerCalled = false;
            m_Player = PlayerManager.Instance.m_Player.transform;
        }

        public virtual void Update()
        {
            m_StateTimer -= Time.deltaTime;
        }
 
        public virtual void Exit()
        {
            m_Animator.SetBool(m_AnimationHash, false);
            m_EnemyBase.AssignLastAnimationName(m_AnimationHash);
        }
        
        public virtual void AnimationFinishTrigger()
        {
            m_TriggerCalled = true;
        }

        public AnimatorStateInfo GetAnimatorStateInfo()
        {
            return m_Animator.GetCurrentAnimatorStateInfo(0);
        }
    }
}
