using System.Collections.Generic;
using UnityEngine;

namespace Player.Skills.Sword
{
    public class Sword_SkillController : MonoBehaviour
    {
        [SerializeField] private float m_ReturnSpeed = 12f;
        [SerializeField] private float m_DestroyThreshold = 0.5f;
        private Animator m_Animator;
        private Rigidbody2D m_Rigidbody2D;
        private CircleCollider2D m_CircleCollider2D;
        private Player m_Player;

        private bool m_CanRotate;
        private bool m_IsReturning;

        [SerializeField] private int m_NumberOfBounces = 4;
        [SerializeField] private float m_BouncingRadius;
        [SerializeField] private float m_BounceSpeed;
        [SerializeField] private float m_ChangeIndexDistance;
        [SerializeField] private LayerMask m_EnemyLayerMask;
        private bool m_IsBouncing = true;
        private List<Transform> m_EnemyTarget = new List<Transform>(10);
        private int m_TargetIndex;
        
        private void Awake()
        {
            m_CanRotate = true;
            m_IsReturning = false;
            m_Animator = GetComponentInChildren<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
        }

        public void SetUpSword(Vector2 _direction, float _gravityScale, Player _player)
        {
            m_Player = _player;
            m_Rigidbody2D.velocity = _direction;
            m_Rigidbody2D.gravityScale = _gravityScale;
            
            m_Animator.SetBool(EntityStatesAnimationHash.SWORD_ROTATE, true);
        }

        public void ReturnSword()
        {
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll; 
           // m_Rigidbody2D.isKinematic = false;
            transform.parent = null;
            m_IsReturning = true;
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
                transform.position = Vector2.MoveTowards(transform.position, m_Player.transform.position,
                    m_ReturnSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, m_Player.transform.position) < m_DestroyThreshold)
                {
                    m_Player.CatchSword();
                }
            }

            if (m_IsBouncing && m_EnemyTarget.Count > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_EnemyTarget[m_TargetIndex].position,
                    m_BounceSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, m_EnemyTarget[m_TargetIndex].position) < m_ChangeIndexDistance)
                {
                    m_TargetIndex++;
                    m_NumberOfBounces--;

                    if (m_NumberOfBounces == 0)
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

            if (other.GetComponent<Enemy.Enemy>() != null)
            {
                if (m_IsBouncing && m_EnemyTarget.Count <= 0)
                {
                    Collider2D[] colliders =
                        Physics2D.OverlapCircleAll(transform.position, m_BouncingRadius, m_EnemyLayerMask);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy.Enemy>() != null)
                        {
                            m_EnemyTarget.Add(hit.transform);
                        }
                    }
                }
            }
            
            StuckInEnemy(other);

        }
        
        private void StuckInEnemy(Collider2D other)
        {


            m_CanRotate = false;
            m_CircleCollider2D.enabled = false;

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
