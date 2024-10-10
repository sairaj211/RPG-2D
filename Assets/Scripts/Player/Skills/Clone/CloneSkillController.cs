using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Skills
{
    public class CloneSkillController : MonoBehaviour
    {
        [SerializeField] private float m_ColorFadeSpeed;

        [SerializeField] private Transform m_AttackCheck;
        [SerializeField] private float m_AttackCheckRadius = 1f;
        [SerializeField] private LayerMask m_EnemyLayerMask;
        private SpriteRenderer m_SpriteRenderer;
        private Animator m_Animator;
        private float m_CloneTimer;

        private Transform m_ClosestEnemy;
        private bool m_CanDuplicateClone;
        private int m_FacingDirection = 1;
        private int m_PercentageChance;

        // Angular speed in radians per sec.
        public float m_Speed = 20.0f;
        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Animator = GetComponent<Animator>();
        }

        private void Update()
        {
            m_CloneTimer -= Time.deltaTime;
            float alpha = Mathf.Lerp(m_SpriteRenderer.color.a, 0.0f, Time.deltaTime);
            m_SpriteRenderer.color = new Color(1, 1, 1, alpha);
            
            if (m_CloneTimer < -2f)
            {
              //  m_SpriteRenderer.color = new Color(1, 1, 1, m_SpriteRenderer.color.a - (Time.deltaTime - m_ColorFadeSpeed));
                Destroy(gameObject);
            }
        }

        public void SetUpClone(Transform _newTransform, float _cloneDuration, bool _CanAttack, bool _duplicate, int _chance, Transform _closestEnemy, Vector3 _offset = default)
        {
            if (_CanAttack)
            {
                m_Animator.SetInteger(EntityStatesAnimationHash.ATTACK_NUMBER, Random.Range(1, 4));
            }

            transform.position = _newTransform.position + _offset;
            m_CloneTimer = _cloneDuration;
            m_CanDuplicateClone = _duplicate;
            m_ClosestEnemy = _closestEnemy;
            m_PercentageChance = _chance;
            FaceClosestTarget();
        }

        private void FaceClosestTarget()
        {
            if (m_ClosestEnemy != null)
            {
                /*//Vector2 directionToPlayer = (transform.position - m_ClosestEnemy.transform.position).normalized;
                
                // Determine which direction to rotate towards
                Vector3 targetDirection = m_ClosestEnemy.position - transform.position;

                // The step size is equal to speed times frame time.
                float singleStep = m_Speed * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
                
                // Calculate a rotation a step closer to the target and applies rotation to this object
                transform.rotation = Quaternion.LookRotation(newDirection);*/

                if (transform.position.x > m_ClosestEnemy.position.x)
                {
                    m_FacingDirection = -1;
                    transform.Rotate(0,180f,0);
                }
            }
        }
        
        
        private void AnimationTrigger()
        {
            m_CloneTimer = -.1f;
        }

        private void AttackTrigger()
        {
            Collider2D[] m_Collider2D = Physics2D.OverlapCircleAll(m_AttackCheck.position, m_AttackCheckRadius, m_EnemyLayerMask);

            foreach (Collider2D hit in m_Collider2D)
            {
                if (hit.TryGetComponent<Enemy.Enemy>(out var enemy))
                {
                    enemy.DamageEffect();
                    if (m_CanDuplicateClone)
                    {
                        int m_Chance = Random.Range(0, 100);
                        if (m_Chance < m_PercentageChance)
                        {
                            SkillManager.Instance.m_CloneSKill.CreateClone(hit.transform, new Vector3(0.25f * m_FacingDirection, 0f));
                        }
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(m_AttackCheck.position, m_AttackCheckRadius);
        }
    }
}
