using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.Skills.Sword
{
    public class SwordSkillController : MonoBehaviour
    {
        [SerializeField] private float m_MaxReturnSpeed = 12f;
        [SerializeField] private float m_MinReturnSpeed = 12f;
        [SerializeField] private float m_DestroyThreshold = 0.5f;
        [SerializeField] private float m_FreezeTime;
        [SerializeField] private float m_MaxDistance;
        [SerializeField] private float m_MinDistace;
        private Animator m_Animator;
        private Rigidbody2D m_Rigidbody2D;
        private BoxCollider2D m_BoxCollider2D;
        private Player m_Player;

        private bool m_CanRotate;
        private bool m_IsReturning;

        [Header("Bounce Info")]
        [SerializeField] private float m_BounceSpeed;
        [SerializeField] private float m_BouncingRadius;
        [SerializeField] private LayerMask m_EnemyLayerMask;
        [SerializeField] private float m_ChangeIndexDistance;
        private int m_NumberOfBounces;
        private bool m_IsBouncing;
        private List<Transform> m_EnemyTarget = new List<Transform>(10);
        private int m_TargetIndex;

        [Header("Pierce Info")] 
        private float m_NumberOfPierces;

        [Header("Spin Sword Info")] 
        [SerializeField] private float m_HitRadius;

        [SerializeField] private float m_MoveSpeed;
        private float m_MaxTravelDistance;
        private float m_SpinDuration;
       // private float m_SpinTimer;
        private bool m_WasStopped;
        private bool m_IsSpinning;
        private float m_HitTimer;
        private float m_HitCooldown;
        private float m_SpinDirection;

        private float m_CurrentSpeed;

        
        private void Awake()
        {
            m_CanRotate = true;
            m_IsReturning = false;
            m_Animator = GetComponentInChildren<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();
        }

        public void SetUpSword(Vector2 _direction, float _gravityScale, Player _player)
        {
            m_Player = _player;
            m_Rigidbody2D.velocity = _direction;
            m_Rigidbody2D.gravityScale = _gravityScale;

            m_SpinDirection = Mathf.Clamp(m_Rigidbody2D.velocityX, -1f, 1f);
        }

        public void SetupBounceSword(bool _isBouncing, int _numberOfBounces)
        {
            m_IsBouncing = _isBouncing;
            m_NumberOfBounces = _numberOfBounces;
            m_Animator.SetBool(EntityStatesAnimationHash.SWORD_ROTATE, true);
        }
        
        public void SetupPierceSword(int _numberOfPierces)
        {
            m_NumberOfPierces = _numberOfPierces;
        }

        public void SetupSpinningSword(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
        {
            m_IsSpinning = _isSpinning;
            m_MaxTravelDistance = _maxTravelDistance;
            m_SpinDuration = _spinDuration;
            m_HitCooldown = _hitCooldown;
            //m_SpinTimer = m_SpinDuration;
            m_Animator.SetBool(EntityStatesAnimationHash.SWORD_ROTATE, true);
        }

        public void ReturnSword()
        {
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll; 
           // m_Rigidbody2D.isKinematic = false;
            transform.parent = null;
            m_IsReturning = true;
            
            if(m_IsBouncing)
                m_Animator.SetBool(EntityStatesAnimationHash.SWORD_ROTATE, true);
        }
        
        private void Update()
        {
            if (m_CanRotate)
            {
                transform.right = m_Rigidbody2D.velocity;
            }

            if (m_IsReturning)
            {
                var m_Distance = Vector2.Distance(transform.position, m_Player.transform.position);
                
                m_CurrentSpeed = m_MinReturnSpeed;
                if (m_Distance > m_MinDistace)
                {
                    float t = Mathf.InverseLerp(m_MaxDistance, m_MinDistace, m_Distance);
                    m_CurrentSpeed = Mathf.Lerp(m_MaxReturnSpeed, m_MinReturnSpeed, t);
                }
                
                transform.position = Vector2.MoveTowards(transform.position, m_Player.transform.position,
                    m_CurrentSpeed * Time.deltaTime);

               
                if (m_Distance < m_DestroyThreshold)
                {
                    m_Player.CatchSword();
                }
            }

            BounceLogic();

            SpinLogic();
        }

        private void SpinLogic()
        {
            if (m_IsSpinning)
            {
                if (Vector2.Distance(transform.position, m_Player.transform.position) > m_MaxTravelDistance
                    && !m_WasStopped)
                {
                    StopWhenHit();
                }

                if (m_WasStopped)
                {
                    UpdateSpinPosition();
                    
                    HandleSpinDuration();

                    HandleHitTimer();
                }
            }
        }
        
        private void UpdateSpinPosition()
        {
            m_SpinDuration -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(
                transform.position.x + m_SpinDirection, transform.position.y), m_MoveSpeed * Time.deltaTime);
        }
        
        private void HandleSpinDuration()
        {
            if (m_SpinDuration < 0f)
            {
                m_IsReturning = true;
                m_IsSpinning = false;
            }
        }
        
        private void HandleHitTimer()
        {
            m_HitTimer -= Time.deltaTime;
            if (m_HitTimer < 0f)
            {
                m_HitTimer = m_HitCooldown;
                DamageEnemiesInRange();
            }
        }
        
        private void StopWhenHit()
        {

            m_WasStopped = true;
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        private void BounceLogic()
        {
            if (m_IsBouncing && m_EnemyTarget.Count > 0)
            {
                Transform target = m_EnemyTarget[m_TargetIndex];
                transform.position = Vector2.MoveTowards(transform.position, target.position, m_BounceSpeed * Time.deltaTime);
                
                if (Vector2.Distance(transform.position, m_EnemyTarget[m_TargetIndex].position) < m_ChangeIndexDistance)
                {
                    DealDamage(m_EnemyTarget[m_TargetIndex].GetComponent<Enemy.Enemy>());
                    m_TargetIndex++;
                    m_NumberOfBounces--;

                    if (m_NumberOfBounces <= 0)
                    {
                        m_IsBouncing = false;
                        m_IsReturning = true;
                    }

                    if (m_TargetIndex >= m_EnemyTarget.Count)
                    {
                        m_TargetIndex = 0;
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(m_IsReturning)   return;
            if(m_IsSpinning)    return;
            
            if (other.TryGetComponent(out Enemy.Enemy enemy))
            {
                DealDamage(enemy);
            }
            
            GetTargetsInBouncingRadius();
            StuckInEnemy(other);
        }

        private void DealDamage(Enemy.Enemy enemy)
        {
            enemy.Damage(m_FreezeTime);
        }

        private void GetTargetsInBouncingRadius()
        {
            if (m_IsBouncing && m_EnemyTarget.Count <= 0)
            {
                Collider2D[] colliders =
                    Physics2D.OverlapCircleAll(transform.position, m_BouncingRadius, m_EnemyLayerMask);

                foreach (var hit in colliders)
                {
                    if (hit.TryGetComponent(out Enemy.Enemy enemy))
                    {
                        m_EnemyTarget.Add(hit.transform);
                    }
                }
            }
        }
        
        private void DamageEnemiesInRange()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_HitRadius, m_EnemyLayerMask);

            foreach (var hit in colliders)
            {
                if (hit.TryGetComponent(out Enemy.Enemy enemy))
                {
                    DealDamage(enemy);
                }
            }
        }

        private void StuckInEnemy(Collider2D other)
        {
            if (m_NumberOfPierces > 0 && other.GetComponent<Enemy.Enemy>() != null)
            {
                m_NumberOfPierces--;
                return;
            }

            if (m_IsSpinning)
            {
                StopWhenHit();
                return;
            }
            
            m_CanRotate = false;
            m_BoxCollider2D.enabled = false;

            m_Rigidbody2D.isKinematic = true;
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            if (m_IsBouncing && m_EnemyTarget.Count > 0)
            {
                return;
            }
            
            m_Animator.SetBool(EntityStatesAnimationHash.SWORD_ROTATE, false);
            transform.parent = other.transform;
        }
    }
}
