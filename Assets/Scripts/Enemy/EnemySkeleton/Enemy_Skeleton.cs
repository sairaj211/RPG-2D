namespace Enemy.EnemySkeleton
{
    public class Enemy_Skeleton : Enemy
    {

        #region States

        public SkeletonIdleState m_IdleState { get; private set; }
        public SkeletonMoveState m_MoveState { get; private set; }
        public SkeletonBattleState m_BattleState { get; private set; }
        public SkeletonAttackState m_AttackState { get; private set; }
        

        #endregion
        
        protected override void Awake()
        {
            base.Awake();
            m_IdleState = new SkeletonIdleState(this, m_EnemyStateMachine, PlayerStatesAnimationHash.IDLE, this);
            m_MoveState = new SkeletonMoveState(this, m_EnemyStateMachine, PlayerStatesAnimationHash.MOVE, this);
            m_BattleState = new SkeletonBattleState(this, m_EnemyStateMachine, PlayerStatesAnimationHash.MOVE, this);
            m_AttackState = new SkeletonAttackState(this, m_EnemyStateMachine, PlayerStatesAnimationHash.ATTACK, this);
        }

        protected override void Start()
        {
            base.Start();
            m_EnemyStateMachine.Initialize(m_IdleState);
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
