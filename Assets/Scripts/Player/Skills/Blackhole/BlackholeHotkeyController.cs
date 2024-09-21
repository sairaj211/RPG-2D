using System;
using TMPro;
using UnityEngine;

namespace Player.Skills.Blackhole
{
    public class BlackholeHotkeyController : MonoBehaviour
    {
        private KeyCode m_Hotkey;
        private TextMeshProUGUI m_Text;
        private SpriteRenderer m_SpriteRenderer;

        private Transform m_Enemy;
        private BlackholeSkillController m_BlackholeSkillController;

        public void SetupHotkey(KeyCode _keyCode, Transform _transform, BlackholeSkillController _controller)
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_Text = GetComponentInChildren<TextMeshProUGUI>();

            m_Enemy = _transform;
            m_BlackholeSkillController = _controller;

            m_Hotkey = _keyCode;
            m_Text.text = m_Hotkey.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(m_Hotkey))
            {
                m_BlackholeSkillController.AddEnemyToList(m_Enemy);
                
                m_Text.color = Color.clear;
                m_SpriteRenderer.color = Color.clear;
            }
        }

        private void OnEnable()
        {
            BlackholeSkillController.OnHotKeyPressedEvent += BlackholeSkillControllerOnOnHotKeyPressedEvent;
        }

        private void OnDisable()
        {
            BlackholeSkillController.OnHotKeyPressedEvent -= BlackholeSkillControllerOnOnHotKeyPressedEvent;
        }

        private void BlackholeSkillControllerOnOnHotKeyPressedEvent()
        {
            Destroy(gameObject);
        }
    }
}
