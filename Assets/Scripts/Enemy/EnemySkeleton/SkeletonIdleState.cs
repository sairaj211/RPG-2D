namespace Enemy.EnemySkeleton
{
    public class SkeletonIdleState : SkeletonGroundedState
    {
        public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, Enemy_Skeleton _skeleton) 
            : base(_enemyBase, _enemyStateMachine, _animHash, _skeleton)
        {
        }

        public override void Enter()
        {
            base.Enter();
            m_StateTimer = m_Enemy.m_IdleTime;
        }

        public override void Update()
        {
            base.Update();

            if (m_StateTimer < 0f)
            {
                m_enemyStateMachine.ChangeState(m_Enemy.m_MoveState);
            }
        }
    }
}
