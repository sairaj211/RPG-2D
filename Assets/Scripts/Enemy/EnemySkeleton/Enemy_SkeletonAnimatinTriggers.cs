using UnityEngine;

namespace Enemy.EnemySkeleton
{
    public class Enemy_SkeletonAnimatinTriggers : MonoBehaviour
    {
        private Enemy m_Enemy => GetComponentInParent<Enemy>();

        private void AnimationTrigger()
        {
            m_Enemy.AnimationTrigger();
        }
        
        private void AttackTrigger()
        {
            Collider2D[] m_Collider2D = Physics2D.OverlapCircleAll(m_Enemy.m_AttackCheck.position, m_Enemy.m_AttackCheckRadius);

            foreach (Collider2D hit in m_Collider2D)
            {
                if (hit.TryGetComponent<Player>(out var player))
                {
                    player.Damage();
                }
            }
        }

        private void OpenCounterAttackWindow() => m_Enemy.OpenCounterAttackWindow();
        private void CloseCounterAttackWindow() => m_Enemy.CloseCounterAttackWindow();
    }
}
