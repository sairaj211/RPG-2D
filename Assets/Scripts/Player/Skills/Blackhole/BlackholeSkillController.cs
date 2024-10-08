using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Skills.Blackhole
{
    public class BlackholeSkillController : MonoBehaviour
    {
        public static event Action OnHotKeyPressedEvent;
        
        [SerializeField] private GameObject m_HotkeyPrefab;
        [SerializeField] private List<KeyCode> m_KeycodeList;

        private float m_MaxSize;
        private float m_GrowSpeed;
        private readonly bool m_CanGrow = true;
        private bool m_CanShrink = false;

        private readonly List<Transform> m_Targets = new List<Transform>(10);

        private int m_NumberOfAttacks;
        private float m_CloneAttackCooldown;
        private float m_CloneAttackTimer;
        private bool m_CreateClonesAndAttack = false;
        private float m_BlackholeTimer;
        public bool m_PlayerCanExitState { get; private set; }

        public void SetupBlackhole(float _maxSize, float _growSpeed, int _numberOfAttacks, float _cloneAttackCooldown, float _duration)
        {
            m_MaxSize = _maxSize;
            m_GrowSpeed = _growSpeed;
            m_NumberOfAttacks = _numberOfAttacks;
            m_CloneAttackCooldown = _cloneAttackCooldown;
            m_BlackholeTimer = _duration;
        }
        
        private void Update()
        {
            m_CloneAttackTimer -= Time.deltaTime;
            m_BlackholeTimer -= Time.deltaTime;

            if (m_BlackholeTimer < 0f)
            {
                m_BlackholeTimer = Mathf.Infinity;

                if (m_Targets.Count > 0)
                {
                    ReleaseCloneAttack();
                }
                else
                {
                    OnHotKeyPressedEvent?.Invoke();
                    StopBlackHole();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.R) && !m_CanShrink)
            {
                ReleaseCloneAttack();
                OnHotKeyPressedEvent?.Invoke();
            }
            
            CloneAttackLogic();
            
            GrowOrShrinkLogic();
        }

        private void ReleaseCloneAttack()
        {
            if(m_Targets.Count <= 0) return;
            
            PlayerManager.Instance.m_Player.MakeTransparent(true);
            m_CreateClonesAndAttack = true;
        }

        private void GrowOrShrinkLogic()
        {

            if (m_CanGrow && !m_CanShrink)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(m_MaxSize, m_MaxSize),
                    m_GrowSpeed * Time.deltaTime);
            }

            if (m_CanShrink)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1f, -1f),
                    m_GrowSpeed * 3f * Time.deltaTime);
                if (transform.localScale.x < 0f)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void CloneAttackLogic()
        {
            if (m_NumberOfAttacks > 0)
            {
                if (m_CreateClonesAndAttack && m_CloneAttackTimer < 0f)
                {
                    m_CloneAttackTimer = m_CloneAttackCooldown;

                    int randomIndex = Random.Range(0, m_Targets.Count);

                    float offset = Random.Range(0, 100) > 50 ? 1f : -1f;

                    SkillManager.Instance.m_CloneSKill.CreateClone(m_Targets[randomIndex], new Vector3(offset, 0));
                    m_NumberOfAttacks--;

                    if (m_NumberOfAttacks <= 0)
                    {
                        Invoke(nameof(StopBlackHole), 0.75f);
                    }
                }
            }
        }

        private void StopBlackHole()
        {
            m_PlayerCanExitState = true;
            m_CreateClonesAndAttack = false;
            m_CanShrink = true;
            //PlayerManager.Instance.m_Player.ExitBlackhole();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Enemy.Enemy m_Enemy))
            {
                m_Enemy.FreezeTime(true);

                if(!m_CreateClonesAndAttack)
                    CreateHotKey(collider);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Enemy.Enemy m_Enemy))
            {
                m_Enemy.FreezeTime(false);
            }
        }

        private void CreateHotKey(Collider2D _collider2D)
        {
            if (m_KeycodeList.Count <= 0)
            {
                Debug.LogWarning("Not enough hotkeys");
                return;
            }

            GameObject newHotkey = Instantiate(m_HotkeyPrefab, _collider2D.transform.position + new Vector3(0, 2f),
                Quaternion.identity);

            KeyCode choosenKey = m_KeycodeList[Random.Range(0, m_KeycodeList.Count)];
            m_KeycodeList.Remove(choosenKey);

            BlackholeHotkeyController hotkeyController = newHotkey.GetComponent<BlackholeHotkeyController>();
            hotkeyController.SetupHotkey(choosenKey, _collider2D.transform, this);
        }

        public void AddEnemyToList(Transform _enemy) => m_Targets.Add(_enemy);
    }
}
