using System;
using UnityEngine;

namespace Misc
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private float m_ParallaxEffect;
        private float m_XPosition;
        private GameObject m_Camera;

        private Transform m_CachedTransform;
        
        // make parallax bg endless
        private float m_Length;

        private void Awake()
        {
            m_CachedTransform = transform;
            m_Camera = GameObject.FindWithTag("MainCamera");
            m_Length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Start()
        {
            m_XPosition = m_CachedTransform.position.x;
        }

        private void Update()
        {
            float distanceMoved = m_Camera.transform.position.x - (1 - m_ParallaxEffect);
            float distanceToMove = m_Camera.transform.position.x * m_ParallaxEffect;
            
            m_CachedTransform.position = new Vector3(m_XPosition + distanceToMove, m_CachedTransform.position.y);
            
            //Endless
            if (distanceMoved > m_XPosition + m_Length)
            {
                m_XPosition += m_Length;
            }
            else if (distanceMoved < m_XPosition - m_Length)
            {
                m_XPosition -= m_Length;
            }
        }
    }
}
