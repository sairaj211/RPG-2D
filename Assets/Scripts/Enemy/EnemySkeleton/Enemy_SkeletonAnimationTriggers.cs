﻿using Player;
using UnityEngine;

namespace Enemy.EnemySkeleton
{
    public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
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
                if (hit.TryGetComponent<Player.Player>(out var player))
                {
                    PlayerStats _targetStats = player.GetComponent<PlayerStats>();
                    m_Enemy.m_CharacterStats.CalculateAndApplyDamage(_targetStats);
                }
            }
        }

        private void OpenCounterAttackWindow() => m_Enemy.OpenCounterAttackWindow();
        private void CloseCounterAttackWindow() => m_Enemy.CloseCounterAttackWindow();
    }
}
