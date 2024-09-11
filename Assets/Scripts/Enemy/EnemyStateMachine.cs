using UnityEngine;

namespace Enemy
{
    public class EnemyStateMachine 
    {
        public EnemyState m_CurrentState { get; private set; }

        public void Initialize(EnemyState _startState)
        {
            m_CurrentState = _startState;
            m_CurrentState.Enter();
        }
        
        public void ChangeState(EnemyState _newState)
        {
            m_CurrentState.Exit();
            m_CurrentState = _newState;
            m_CurrentState.Enter();
        }
    }
}
