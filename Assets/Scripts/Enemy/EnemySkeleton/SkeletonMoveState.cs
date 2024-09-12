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
            
            m_EnemySkeleton.SetVelocity(m_EnemySkeleton.m_MoveSpeed * m_EnemySkeleton.m_FacingDireciton, base.m_Rigidbody2D.velocityY);
            
            if (m_EnemySkeleton.IsWallDetected() || !m_EnemySkeleton.IsGrounded())
            {
                m_EnemySkeleton.Flip();
                m_enemyStateMachine.ChangeState(m_EnemySkeleton.m_IdleState);
            }
        }

    }
}
