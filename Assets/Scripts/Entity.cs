using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")] 
    [SerializeField] protected Transform m_GroundCheck;
    [SerializeField] protected float m_GroundCheckDistance;
    [SerializeField] protected Transform m_WallCheck;
    [SerializeField] protected float m_WallCheckDistance;
    [SerializeField] protected LayerMask m_GroundLayerMask;
    
    #region COMPONENTS
    [Header("References")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    #endregion

    private Vector2 m_Movement;
    private float m_movementSpeed;
    
    public int m_FacingDireciton { private set; get; } = 1;
    protected bool m_IsFacingRight = true;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        
    }
    
    protected virtual void Update()
    {
        
    }
    
    #region SETTERS

    public void SetZeroVelocity()
    {
    //    m_Movement = new Vector2(0f, 0f).normalized;
      //  Movement();
     m_Rigidbody2D.velocity = new Vector2(0f, 0f);
    }

    // }

    public void SetVelocity(float _x, float _y)
    {
        FlipController(_x);
        m_Rigidbody2D.velocity = new Vector2(_x, _y);
     //  m_Movement = new Vector2(_x, _y).normalized;
    //   Movement();
    }
    #endregion

    protected void SetSpeed(float _moveSpeed)
    {
        this.m_movementSpeed = _moveSpeed;
    }
    
    private void Movement()
    {
        m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + m_Movement * (m_movementSpeed * Time.fixedDeltaTime));
    }

    #region GETTERS
    public Animator GetAnimator()
    {
        return m_Animator;
    }

    public Rigidbody2D GetRigidbody()
    {
        return m_Rigidbody2D;
    }
    #endregion
    
    #region COLLISIONS
    public bool IsGrounded() =>
        Physics2D.Raycast(m_GroundCheck.position, Vector2.down, m_GroundCheckDistance, m_GroundLayerMask);

    public bool IsWallDetected() =>
        Physics2D.Raycast(m_WallCheck.position, Vector2.right * m_FacingDireciton, m_WallCheckDistance, m_GroundLayerMask);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_GroundCheck.position, new Vector3(m_GroundCheck.position.x, m_GroundCheck.position.y - m_GroundCheckDistance));
        Gizmos.DrawLine(m_WallCheck.position, new Vector3(m_WallCheck.position.x + m_WallCheckDistance, m_WallCheck.position.y));
    }
    #endregion
    
    #region FLIP
    public void Flip()
    {
        m_FacingDireciton *= -1;
        m_IsFacingRight = !m_IsFacingRight;
        transform.Rotate(0, 180, 0);
    }
    public void FlipController(float _XDirection)
    {
        if (_XDirection > 0 && !m_IsFacingRight)
        {
            Flip();
        }
        else if (_XDirection < 0 && m_IsFacingRight)
        {
            Flip();
        }
    }
    #endregion
}
