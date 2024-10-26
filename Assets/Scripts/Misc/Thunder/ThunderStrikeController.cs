using System;
using Unity.Mathematics;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats m_TargetStats;
    [SerializeField] private float m_Speed;
    private int m_Damage;

    private Animator m_Animator;
    private bool m_Triggered;

    private void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Triggered = false;
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        m_Damage = _damage;
        m_TargetStats = _targetStats;
    }

    private void Update()
    {
        if (!m_TargetStats || m_Triggered)
        {
            return;
        }

        transform.position =
            Vector3.MoveTowards(transform.position, m_TargetStats.transform.position, m_Speed * Time.deltaTime);
        transform.right = transform.position - m_TargetStats.transform.position;

        if (Vector2.Distance(transform.position, m_TargetStats.transform.position) < 0.1f)
        {
            m_Animator.transform.localPosition = new Vector3(0f, 0.5f);
            m_Animator.transform.localRotation= Quaternion.identity;
            
            transform.localRotation = quaternion.identity;
            transform.localScale = new Vector3(3f, 3f);

            m_Triggered = true;
            m_Animator.SetTrigger(EntityStatesAnimationHash.THUNDER_HIT);
        }
    }

    public void DamageAndSelfDestroy()
    {
        //TODO
        // need to pass shock duration after chain lightning
        m_TargetStats.ApplyShockWithoutTimer(); 
        
        m_TargetStats.TakeDamage(m_Damage);
        Destroy(gameObject, 0.4f);
    }
}
