namespace Enemy.EnemySkeleton
{
    public class SkeletonGroundedState : EnemyState
    {
        protected readonly Enemy_Skeleton m_Enemy;

        protected SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash, Enemy_Skeleton _skeleton) 
            : base(_enemyBase, _enemyStateMachine, _animHash)
        {
            m_Enemy = _skeleton;
        }

        public override void Update()
        {
            base.Update();

            if (m_Enemy.IsPlayerDetected())
            {
                m_enemyStateMachine.ChangeState(m_Enemy.m_BattleState);
            }
        }
    }
}
