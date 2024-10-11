using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player.Player _player, PlayerStateMachine _playerStateMachine, int _animHash) 
        : base(_player, _playerStateMachine, _animHash)
    {
    }

    public override void Update()
    {
        base.Update();
        m_Player.DisableCollider();
        m_Player.SetZeroVelocity();
    }
}
