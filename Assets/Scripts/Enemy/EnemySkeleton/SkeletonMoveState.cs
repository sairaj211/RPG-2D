namespace Enemy.EnemySkeleton
{
    public class SkeletonMoveState : SkeletonGroundedState
    {
        public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, Enemy_Skeleton _skeleton) 
            : base(_enemyBase, _enemyStateMachine, _animHash, _skeleton)
        {
        }
        public override void Update()
        {
            base.Update();
            
            m_Enemy.SetVelocity(m_Enemy.m_MoveSpeed * m_Enemy.m_FacingDireciton, base.m_Rigidbody2D.velocityY);
            
            if (m_Enemy.IsWallDetected() || !m_Enemy.IsGrounded())
            {
                m_Enemy.Flip();
                m_enemyStateMachine.ChangeState(m_Enemy.m_IdleState);
            }
        }

    }
}
