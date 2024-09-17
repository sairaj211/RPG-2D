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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(m_IsReturning)   return;
            
            m_Animator.SetBool(EntityStatesAnimationHash.SWORD_ROTATE, false);

            m_CanRotate = false;
            m_CircleCollider2D.enabled = false;

            m_Rigidbody2D.isKinematic = true;
            m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            transform.parent = other.transform;
        }
    }
}
