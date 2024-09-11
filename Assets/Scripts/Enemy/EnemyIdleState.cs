namespace Enemy
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash) 
            : base(_enemyBase, _enemyStateMachine, _animHash)
        {
        }
        
        
    }
}
