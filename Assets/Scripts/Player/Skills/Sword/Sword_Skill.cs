using System;
using Misc;
using Player.Skills.Sword;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Player.Skills
{

    public enum SwordType
    {
        Regular,
        Bounce,
        Pierce,
        Spin
    }
    
    public class Sword_Skill : Skill
    {
        public SwordType m_SwordType;
        
        [Header("Bounce Info")] 
        [SerializeField] private int m_NumberOfBounces;
        [SerializeField] private float m_BounceGravity;

        [Header("Pierce Info")] 
        [SerializeField] private int m_NumberOfPierces; 
        [SerializeField] private float m_PierceGravity;
        
        [Header("Spinning Info")] 
        [SerializeField] private float m_MaxTravelDistance;
        [SerializeField] private float m_SpinDuration;
        [SerializeField] private float m_SpinGravity;
        [SerializeField] private int m_NumberOfHitsPerSecond;

        
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

            switch (m_SwordType)
            {
                case SwordType.Bounce:
                    m_SwordGravity = m_BounceGravity;
                    m_SwordSkillController.SetupBounceSword(true, m_NumberOfBounces);
                    break;
                case SwordType.Pierce:
                    m_SwordGravity = m_PierceGravity;
                    m_SwordSkillController.SetupPierceSword(m_NumberOfPierces);
                    break;
                case SwordType.Spin:
                    m_SwordGravity = m_SpinGravity;
                    m_SwordSkillController.SetupSpinningSword(true, m_MaxTravelDistance, m_SpinDuration, 1f/m_NumberOfHitsPerSecond);
                    break;
            }
            
            m_SwordSkillController.SetUpSword(m_FinalDirection, m_SwordGravity, m_Player);
            
            m_Player.AssignNewSword(m_Sword);
            
            ActivateDots(false);
        }

        #region Aim Region
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
        #endregion
    }
}
