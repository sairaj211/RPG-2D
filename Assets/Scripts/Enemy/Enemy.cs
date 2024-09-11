using System;
using UnityEngine;

namespace Enemy
{
    public class Enemy : Entity
    {
        [Header("Settings")] 
        public float m_MoveSpeed;

        public float m_IdleTime;
        public float m_FacingThreshold = 0.5f;
        public float m_BattleTime; 
        
        [Header("Attack Info")]
        [SerializeField] private float m_Range;
        [SerializeField] private LayerMask m_PlayerLayerMask;
        public float m_AttackDistance;
        public float m_AttackCooldown;
        [HideInInspector] public float m_PreviousAttackTime;
        
        public EnemyStateMachine m_EnemyStateMachine { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            m_EnemyStateMachine = new EnemyStateMachine();
        }

        protected override void Start()
        {
            base.Start();
            base.SetSpeed(m_MoveSpeed);
        }
        
        protected override void Update()
        {           
            base.Update();

            m_EnemyStateMachine.m_CurrentState.Update();
            
            
        }
        
        public void AnimationTrigger() => m_EnemyStateMachine.m_CurrentState.AnimationFinishTrigger();

        public RaycastHit2D IsPlayerDetected()
        {
            return Physics2D.Raycast(m_WallCheck.position, Vector2.right * m_FacingDireciton, m_Range,
                m_PlayerLayerMask);
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + m_AttackDistance * m_FacingDireciton, transform.position.y));
        }
    }
}
