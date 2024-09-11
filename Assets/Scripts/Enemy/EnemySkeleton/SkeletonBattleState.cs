using UnityEngine;

namespace Enemy.EnemySkeleton
{
    public class SkeletonBattleState : EnemyState
    {
        private Enemy_Skeleton m_Enemy;
        private float m_AttackTimer = 0f;

        public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, Enemy_Skeleton _skeleton) 
            : base(_enemyBase, _enemyStateMachine, _animHash)
        {
            m_Enemy = _skeleton;
        }

        public override void Update()
        {
            base.Update();
            m_AttackTimer += Time.deltaTime;
            if (m_Enemy.IsPlayerDetected())
            {
                m_StateTimer = m_Enemy.m_BattleTime;
                if (m_Enemy.IsPlayerDetected().distance < m_Enemy.m_AttackDistance && CanAttack())
                {
                    m_enemyStateMachine.ChangeState(m_Enemy.m_AttackState);
                }
                else
                {
                    m_Enemy.SetZeroVelocity();
                }
            }
            else
            {
                // TODO : ADD MAX DISTANCE (or condition)
                if (m_StateTimer < 0f)
                {
                    m_enemyStateMachine.ChangeState(m_Enemy.m_IdleState);
                }
            }
            
            MoveTowardsPlayer();
        }
        
        private void MoveTowardsPlayer()
        {
            // Calculate direction to the player
            Vector2 directionToPlayer = (m_Player.position - m_Enemy.transform.position).normalized;
            
            m_Enemy.SetVelocity(m_Enemy.m_MoveSpeed * directionToPlayer.x, m_Rigidbody2D.velocityY);
        }
                
        private bool CanAttack()
        {
            if (m_AttackTimer > m_Enemy.m_AttackCooldown)
            {
                m_AttackTimer = 0f;
                return true;
            }

            return false;
        }
        

        private void DotProductTest()
        {
            Vector2 playerToEnemy = (m_Enemy.transform.position - m_Player.position).normalized;
            Vector2 playerFacingDirection = m_Player.right;
            float dotProduct = Vector2.Dot(playerToEnemy, playerFacingDirection);
            if (dotProduct > m_Enemy.m_FacingThreshold)
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
