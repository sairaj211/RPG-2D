using Enemy;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player.Player m_Player => GetComponentInParent<Player.Player>();

    private void AnimationTrigger()
    {
        m_Player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] m_Collider2D = Physics2D.OverlapCircleAll(m_Player.m_AttackCheck.position, m_Player.m_AttackCheckRadius);

        foreach (Collider2D hit in m_Collider2D)
        {
            if (hit.TryGetComponent<Enemy.Enemy>(out var enemy))
            {
                EnemyStats _targetStats = enemy.GetComponent<EnemyStats>();
                m_Player.m_CharacterStats.CalculateDamage(_targetStats);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.Instance.m_SwordSkill.CreateSword();
    }
}
