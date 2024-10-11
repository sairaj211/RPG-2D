using UnityEngine;

namespace Enemy.EnemySkeleton
{
    public class SkeletonBattleState : EnemyState
    {
        private Enemy_Skeleton m_EnemySkeleton;
        private float m_AttackTimer = 0f;

        public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, Enemy_Skeleton _skeleton) 
            : base(_enemyBase, _enemyStateMachine, _animHash)
        {
            m_EnemySkeleton = _skeleton;
        }

        public override void Update()
        {
            base.Update();
            m_AttackTimer += Time.deltaTime;
            if (m_EnemySkeleton.IsPlayerDetected() && m_EnemySkeleton.m_IsPlayerAlive)
            {
                m_StateTimer = m_EnemySkeleton.m_BattleTime;
                if (m_EnemySkeleton.IsPlayerDetected().distance < m_EnemySkeleton.m_AttackDistance && CanAttack())
                {
                    m_enemyStateMachine.ChangeState(m_EnemySkeleton.m_AttackState);
                }
                else
                {
                    m_EnemySkeleton.SetZeroVelocity();
                }
            }
            else
            {
                // TODO : ADD MAX DISTANCE (or condition)
                if (m_StateTimer < 0f)
                {
                    m_enemyStateMachine.ChangeState(m_EnemySkeleton.m_IdleState);
                }
            }

            if (Vector2.Distance(m_EnemySkeleton.transform.position, m_Player.position) > m_EnemySkeleton.m_AttackDistance)
            {
                MoveTowardsPlayer();
            }
        }
        
        private void MoveTowardsPlayer()
        {
            // Calculate direction to the player
            Vector2 directionToPlayer = (m_Player.position - m_EnemySkeleton.transform.position).normalized;
            
            m_EnemySkeleton.SetVelocity(m_EnemySkeleton.m_MoveSpeed * directionToPlayer.x, m_Rigidbody2D.velocityY);
        }
                
        private bool CanAttack()
        {
            if (m_AttackTimer > m_EnemySkeleton.m_AttackCooldown)
            {
                m_AttackTimer = 0f;
                return true;
            }

            return false;
        }
        

        private void DotProductTest()
        {
            Vector2 playerToEnemy = (m_EnemySkeleton.transform.position - m_Player.position).normalized;
            Vector2 playerFacingDirection = m_Player.right;
            float dotProduct = Vector2.Dot(playerToEnemy, playerFacingDirection);
            if (dotProduct > m_EnemySkeleton.m_FacingThreshold)
            {
                Debug.Log("The player is facing the enemy.");
            }
            else
            {
                Debug.Log("The player is not facing the enemy.");
            }
        }
    }
    

}
