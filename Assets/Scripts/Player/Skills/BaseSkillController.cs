using System;
using UnityEngine;

namespace Player.Skills
{
    public class BaseSkillController : MonoBehaviour
    {
        protected Player m_Player;

        private void Start()
        {
            m_Player = PlayerManager.Instance.m_Player;
        }
    }
}
