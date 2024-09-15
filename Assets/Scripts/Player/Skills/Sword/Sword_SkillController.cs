using System;
using UnityEngine;
using Player;

public class Sword_SkillController : MonoBehaviour
{
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody2D;
    private CircleCollider2D m_CircleCollider2D;
    private Player.Player m_Player;

    private void Start()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_CircleCollider2D = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 _direction, float _gravityScale)
    {
        if (m_Rigidbody2D == null)
        {
            GetReferences();
        }
        m_Rigidbody2D.velocity = _direction;
        m_Rigidbody2D.gravityScale = _gravityScale;
    }
}
