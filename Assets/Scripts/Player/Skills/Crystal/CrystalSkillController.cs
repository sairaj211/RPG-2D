using System;
using UnityEngine;

namespace Player.Skills.Crystal
{
    public class CrystalSkillController : MonoBehaviour
    {
        [SerializeField] private bool m_CanGrow;
        [SerializeField] private float m_GrowSpeed;
        
        private float m_CrystalExistTimer;
        private bool m_CanExplode;
        private bool m_CanMove;
        private float m_MoveSpeed;

        private Animator m_Animator;
        private CircleCollider2D m_CircleCollider2D;
        public Transform m_ClosestTarget;
        public float speed = 5f; // Speed of the missile
        public float arcHeight = 2f; // Height of the arc
        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
        }

        public void SetupCrystal(float _duration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget)
        {
            m_CrystalExistTimer = _duration;
            m_CanExplode = _canExplode;
            m_CanMove = _canMove;
            m_MoveSpeed = _moveSpeed;
            m_ClosestTarget = _closestTarget;
        }

        private void Update()
        {
            m_CrystalExistTimer -= Time.deltaTime;

            if (m_CrystalExistTimer < 0f)
            {
                CrystalDestroyLogic();
            }

            if (m_CanMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_ClosestTarget.position,
                    m_MoveSpeed * Time.deltaTime);

           //     TweenMissile();
            
                if (Vector2.Distance(transform.position, m_ClosestTarget.position) < 0.5f)
                {
                    CrystalDestroyLogic();
                    m_CanMove = false;
                }
            }
            
            if (m_CanGrow)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3f, 3f),
                    m_GrowSpeed * Time.deltaTime);
            }
        }

        /*private void TweenMissile()
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = m_ClosestTarget.position;

            // Create a tween that moves the missile in an arc to the target
            transform.DOJump(targetPosition, arcHeight, 1, speed)
                .OnComplete(() => CrystalDestroyLogic());
        }

        private void HitTarget()
        {
            // Logic for what happens when the missile hits the target
            Debug.Log("Missile has hit the target!");
            // You can destroy the missile or trigger effects here
            Destroy(gameObject);
        }*/
        
        public void CrystalDestroyLogic()
        {

            if (m_CanExplode)
            {
                m_CanGrow = true;
                m_Animator.SetTrigger(EntityStatesAnimationHash.CRYSTAL_EXPLODE);
            }
            else
            {
                SelfDestroy();
            }
        }

        private void AnimationExplodeEvent()
        {
            Collider2D[] m_Collider2D = Physics2D.OverlapCircleAll(transform.position, m_CircleCollider2D.radius);

            foreach (Collider2D hit in m_Collider2D)
            {
                if (hit.TryGetComponent<Enemy.Enemy>(out var enemy))
                {
                    enemy.Damage();
                }
            }
        }
        
        private void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}
