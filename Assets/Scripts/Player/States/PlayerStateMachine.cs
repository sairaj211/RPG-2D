using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState m_CurrentState { get; private set; }

    public void Initialize(PlayerState _startState)
    {
        m_CurrentState = _startState;
        m_CurrentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        m_CurrentState.Exit();
        m_CurrentState = _newState;
        m_CurrentState.Enter();
    }
}
