using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Enemy : Entity
    {
    //    protected Action OnEnemyDeathEvent;
        
        [Header("Settings")] 
        public float m_MoveSpeed;
        public float m_IdleTime;
        public float m_FacingThreshold = 0.5f;
        public float m_BattleTime;
        private float m_DefaultMoveSpeed;
        
        [Header("Stun Info")]
        public float m_StunDuration;
        public Vector2 m_StunDirection;
        protected bool m_CanBeStunned;
        [SerializeField] private GameObject m_CounterImage;
        
        [Header("Attack Info")]
        [SerializeField] private float m_Range;
        [SerializeField] private LayerMask m_PlayerLayerMask;
        public float m_AttackDistance;
        public float m_AttackCooldown;
        [HideInInspector] public float m_PreviousAttackTime;
        [HideInInspector] public bool m_IsPlayerAlive;

        public int m_LastAnimationHash;
        
        private float m_EffectTimer;
        private bool m_EffectApplied;
        
        public EnemyStateMachine m_EnemyStateMachine { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            m_EnemyStateMachine = new EnemyStateMachine();
            m_DefaultMoveSpeed = m_MoveSpeed;
        }

        protected override void Start()
        {
            base.Start();
            base.SetSpeed(m_MoveSpeed);
            m_IsPlayerAlive = PlayerManager.Instance.GetPlayerAlive();
        }
        
        protected override void Update()
        {           
            base.Update();

            m_EnemyStateMachine.m_CurrentState.Update();
            
            if (m_EffectApplied)
            {
                m_EffectTimer -= Time.deltaTime;

                if (m_EffectTimer < 0f)
                {
                    m_EffectApplied = false;
                    ResetToDefaultValues();
                }
            }
        }

        public virtual void FreezeTime(bool _timeFrozen)
        {
            if (_timeFrozen)
            {
                m_MoveSpeed = 0f;
                this.GetAnimator().speed = 0f;
            }
            else
            {
                m_MoveSpeed = m_DefaultMoveSpeed;
                this.GetAnimator().speed = 1f;
            }
        }

        public override void ApplySlowEffect(float _slowPercentage, float _slowDuration)
        {
            float slowAmount = 1 - _slowPercentage;

            m_MoveSpeed *= slowAmount;

            GetAnimator().speed *= slowAmount;

            m_EffectTimer = _slowDuration;
            m_EffectApplied = true;
        }

        private void ResetToDefaultValues()
        {
            m_MoveSpeed =  m_DefaultMoveSpeed;
            SetAnimationDefaultSpeed();
        }
        
        // private IEnumerator FreezeTimeFor(float _seconds)
        // {
        //     FreezeTime(true);
        //
        //     yield return new WaitForSeconds(_seconds);
        //     
        //     FreezeTime(false);
        // }
        
        public void AnimationTrigger() => m_EnemyStateMachine.m_CurrentState.AnimationFinishTrigger();

        public virtual void OpenCounterAttackWindow()
        {
            m_CanBeStunned = true;
            m_CounterImage.SetActive(true);
        }
        
        public virtual void CloseCounterAttackWindow()
        {
            m_CanBeStunned = false;
            m_CounterImage.SetActive(false);
        }

        public virtual bool CanBeStunned()
        {
            if (m_CanBeStunned)
            {
                CloseCounterAttackWindow();
                return true;
            }

            return false;
        }
        
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

        // public override void DamageEffect(float _freezeTime)
        // {
        //    // Debug.Log("DAMAGE TAKEN");
        //     
        //     base.DamageEffect();
        //     
        //     StartCoroutine(FreezeTimeFor(_freezeTime));
        // }
        
        // private void OnDeathEventCallback()
        // {
        //     OnEnemyDeathEvent?.Invoke();
        // }

        public virtual void AssignLastAnimationName(int _hash)
        {
            m_LastAnimationHash = _hash;
        }
    }
}
