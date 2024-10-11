using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Player.Player m_Player;
    private bool m_IsPlayerAlive;

    public bool GetPlayerAlive() => m_IsPlayerAlive;

    protected override void Awake()
    {
        base.Awake();
        m_IsPlayerAlive = true;
    }

    private void OnEnable()
    {
        Player.Player.OnPlayerDeathEvent += OnPlayerDeathEvent;
    }

    private void OnDisable()
    {
        Player.Player.OnPlayerDeathEvent -= OnPlayerDeathEvent;
    }

    private void OnPlayerDeathEvent()
    {
        m_IsPlayerAlive = false;
    }
}
