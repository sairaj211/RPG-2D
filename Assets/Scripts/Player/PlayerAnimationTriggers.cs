using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player m_Player => GetComponentInParent<Player>();

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
                enemy.Damage();
            }
        }
    }
}