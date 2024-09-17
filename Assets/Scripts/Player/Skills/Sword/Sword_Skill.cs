using System;
using Misc;
using Player.Skills.Sword;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Player.Skills
{
    public class Sword_Skill : Skill
    {
        [Header("Skill Info")] 
        [SerializeField] private GameObject m_SwordPrefab;

        [SerializeField] private Vector2 m_LaunchForce;
        [SerializeField] private float m_SwordGravity;

        private Vector2 m_FinalDirection;

        [Header("Aim dots")] 
        [SerializeField] private int m_NumberofDots;

        [SerializeField] private float m_SpaceBetweenDots;
        [SerializeField] private GameObject m_DotsPrefab;
        [SerializeField] private Transform m_DotsParent;

        private GameObject[] m_Dots;

        private void Awake()
        {
            m_Dots = new GameObject[m_NumberofDots];
        }

        protected override void Start()
        {
            base.Start();
            
            GenerateDots();
        }

        protected override void Update()
        {
            base.Update();
         
            if(Input.GetKeyUp(KeyCode.Mouse1))
            {
                // Get the normalized aim direction once
                Vector2 normalizedAimDirection = AimDirection().normalized;

                // Compute the final direction using Vector2 multiplication
                m_FinalDirection = Vector2.Scale(normalizedAimDirection, m_LaunchForce);
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                for (int i = 0; i < m_NumberofDots; ++i)
                {
                    m_Dots[i].transform.position = DotsPosition(i * m_SpaceBetweenDots);
                }
            }
        }

        public void CreateSword()
        {
            GameObject m_Sword = Instantiate(m_SwordPrefab, m_Player.transform.position, transform.rotation);
            Sword_SkillController m_SwordSkillController = m_Sword.GetComponent<Sword_SkillController>();
            
            m_SwordSkillController.SetUpSword(m_FinalDirection, m_SwordGravity, m_Player);
            
            m_Player.AssignNewSword(m_Sword);
            
            ActivateDots(false);
        }

        public Vector2 AimDirection()
        {
            Vector2 playerPosition = m_Player.transform.position;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - playerPosition;

            return direction;
        }

        private Vector2 DotsPosition(float t)
        {
            Vector2 normalizedAimDirection = AimDirection().normalized;

            // Compute the final direction using Vector2 multiplication
            Vector2 position = (Vector2)m_Player.transform.position  + Vector2.Scale(normalizedAimDirection, m_LaunchForce) 
                * t + 0.5f * (Physics2D.gravity * m_SwordGravity) * (t * t);

            return position;
        }
        
        private void GenerateDots()
        {
            for (int i = 0; i < m_NumberofDots; ++i)
            {
                m_Dots[i] = Instantiate(m_DotsPrefab, m_Player.transform.position, quaternion.identity, m_DotsParent);
                m_Dots[i].SetActive(false);
            }
        }

        public void ActivateDots(bool _isActive)
        {
            for (int i = 0; i < m_NumberofDots; ++i)
            {
                m_Dots[i].SetActive(_isActive);
            }
        }
    }
}
