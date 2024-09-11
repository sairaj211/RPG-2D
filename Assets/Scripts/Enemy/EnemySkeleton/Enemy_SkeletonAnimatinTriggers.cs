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
    }
}
