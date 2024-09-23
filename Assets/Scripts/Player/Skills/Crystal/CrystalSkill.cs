using System;
using System.Collections.Generic;
using Misc;
using Unity.Mathematics;
using UnityEngine;

namespace Player.Skills.Crystal
{
    public class CrystalSkill : Skill
    {
        [SerializeField] private GameObject m_CrystalPrefab;
        [SerializeField] private float m_CrystalDuration;
        [SerializeField] private float m_TargetCheckRadius;


        private GameObject m_CurrentCrystal;

        [Header("Explosive Crystal")] 
        [SerializeField] private bool m_CanExplode;

        [Header("Moving Crystal")] 
        [SerializeField] private bool m_CanMoveTowardsEnemy;
        [SerializeField] private float m_MoveSpeed;

        [Header("Multi Stacking Crystal")] 
        [SerializeField] private bool m_IsMultiStack;
        [SerializeField] private int m_NumberOfStacks;
        [SerializeField] private float m_MultiStackCooldown;
        [SerializeField] private float m_MultiCrystalUseTimeWindow;
        [SerializeField] private List<GameObject> m_CrystalStack;

        private void Awake()
        {
            if (m_IsMultiStack)
            {
                RefillCrystals();
            }
        }
        
        public override void UseSkill()
        {
            base.UseSkill();

            if (UseMultiCrystal())
            {
                return;
            }
            
            if (m_CurrentCrystal == null)
            {
                m_CurrentCrystal = Instantiate(m_CrystalPrefab, m_Player.transform.position, Quaternion.identity);
                CrystalSkillController controller = m_CurrentCrystal.GetComponent<CrystalSkillController>();
                controller.SetupCrystal(m_CrystalDuration, m_CanExplode, m_CanMoveTowardsEnemy, m_MoveSpeed,
                    FindClosetEnemy(m_CurrentCrystal.transform, m_TargetCheckRadius));
            }
            // else
            // {
            //     m_Player.transform.position = m_CurrentCrystal.transform.position;
            //     CrystalSkillController controller = m_CurrentCrystal.GetComponent<CrystalSkillController>();
            //     controller.CrystalDestroyLogic();
            // }
        }
        

        private bool UseMultiCrystal()
        {
            if (m_IsMultiStack)
            {
                if (m_CrystalStack.Count > 0)
                {
                    
                    if (m_CrystalStack.Count == m_NumberOfStacks)
                    {
                        Invoke(nameof(MultiStackResetAbility), m_MultiCrystalUseTimeWindow);
                    }

                    m_Cooldown = 0f;
                    var crystalToSpwan = m_CrystalStack[m_CrystalStack.Count - 1];
                    GameObject newCrystal = Instantiate(crystalToSpwan,
                        m_Player.transform.position, quaternion.identity);
                    m_CrystalStack.Remove(crystalToSpwan);
                    
                    newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(m_CrystalDuration, 
                        m_CanExplode, m_CanMoveTowardsEnemy,
                        m_MoveSpeed, FindClosetEnemy(newCrystal.transform, m_TargetCheckRadius));

                    if (m_CrystalStack.Count <= 0)
                    {
                        // Refill
                        m_Cooldown = m_MultiStackCooldown;
                        RefillCrystals();
                    }
                }
                return true;
            }

            return false;
        }

        private void RefillCrystals()
        {
            int refillAmount = m_NumberOfStacks - m_CrystalStack.Count;
            for (int i = 0; i < refillAmount; ++i)
            {
                m_CrystalStack.Add(m_CrystalPrefab);
            }
        }

        private void MultiStackResetAbility()
        {
            if(m_CooldownTimer > 0f) return;
            m_CooldownTimer = m_MultiStackCooldown;
            RefillCrystals();
        }
    }
}
