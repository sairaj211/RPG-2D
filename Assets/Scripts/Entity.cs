using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")] 
    public Transform m_AttackCheck;
    public float m_AttackCheckRadius;
    [SerializeField] protected Transform m_GroundCheck;
    [SerializeField] protected float m_GroundCheckDistance;
    [SerializeField] protected Transform m_WallCheck;
    [SerializeField] protected float m_WallCheckDistance;
    [SerializeField] protected LayerMask m_GroundLayerMask;
    public SpriteRenderer m_SpriteRenderer { get; private set; }

    [Header("Knock-back Info")] 
    [SerializeField] protected Vector2 m_KnockbackDirection;

    [SerializeField] private float m_KnockbackDuration;
    protected bool m_IsKnocked;
    
    #region COMPONENTS
    [Header("References")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    [HideInInspector] public EntityFX m_EntityFX;
    [HideInInspector] public CharacterStats m_CharacterStats;
    #endregion

    private Vector2 m_frameVelocity;
    private float m_movementSpeed;
    
    public int m_FacingDireciton { private set; get; } = 1;
    protected bool m_IsFacingRight = true;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        m_EntityFX = GetComponent<EntityFX>();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_CharacterStats = GetComponent<CharacterStats>();
    }
    
    protected virtual void Update()
    {
      //  ApplyMovement();
    }

    public virtual void DamageEffect()
    {
        m_EntityFX.StartCoroutine(m_EntityFX.FlashFX());
        StartCoroutine(HitKnockback());
    }

    public void MakeTransparent(bool _isTransparent)
    {
        if (_isTransparent)
        {
            m_SpriteRenderer.color = Color.clear;
        }
        else
        {
            m_SpriteRenderer.color = Color.white;
        }
    }
    
    public virtual void DamageEffect(float _freezeTime){}

    private IEnumerator HitKnockback()
    {
        m_IsKnocked = true;

        m_Rigidbody2D.velocity = new Vector2(m_KnockbackDirection.x * -m_FacingDireciton, m_KnockbackDirection.y);

        yield return new WaitForSeconds(m_KnockbackDuration);

        m_IsKnocked = false;
    }

    public void HandleMovement(float _x)
    {
        FlipController(_x); 

        m_frameVelocity.x = _x;
    }

    public void HandleGravity()
    {
    }

    private void ApplyMovement()
    {
        m_Rigidbody2D.velocity = m_frameVelocity;
    }
    
    #region SETTERS

    public void SetZeroVelocity()
    { 
        if (m_IsKnocked) return;

       // m_frameVelocity = new Vector2(0f, 0f).normalized; 
       // Movement();
        m_Rigidbody2D.velocity = new Vector2(0f, 0f);
    }

    // }

    public void SetVelocity(float _x, float _y, bool _flip = true)
    {
        if (m_IsKnocked) return;

        if (_flip)
        {
            FlipController(_x);
        }

        m_Rigidbody2D.velocity = new Vector2(_x, _y); 
       // m_frameVelocity = new Vector2(_x, _y).normalized; 
     //   Movement();
    }

    public void SetMovement(float _x, float _y)
    {
        FlipController(_x); 
        m_frameVelocity = new Vector2(_x, _y).normalized; 
        Movement();
    }
    
    #endregion

    protected void SetSpeed(float _moveSpeed)
    {
        this.m_movementSpeed = _moveSpeed;
    }
    
    private void Movement()
    {
        m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + m_frameVelocity * (m_movementSpeed * Time.fixedDeltaTime));
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
        Gizmos.DrawWireSphere(m_AttackCheck.position, m_AttackCheckRadius);
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
