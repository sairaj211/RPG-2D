using Enemy;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    public EnemyMoveState(Enemy.Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, int _animHash) 
        : base(_enemyBase, _enemyStateMachine, _animHash)
    {
    }
}
