using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class EntityFX : MonoBehaviour
    {
        private SpriteRenderer m_SpriteRenderer;
        
        [SerializeField] private Material m_HitMaterial;
        private Material m_OriginalMaterial;

        [SerializeField] private float m_FlashTime;
        
        private void Start()
        {
            m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            m_OriginalMaterial = m_SpriteRenderer.material;
        }

        public IEnumerator FlashFX()
        {
            m_SpriteRenderer.material = m_HitMaterial;

            yield return new WaitForSeconds(m_FlashTime);

            m_SpriteRenderer.material = m_OriginalMaterial;
        }

        public void BlinkEffect()
        {
            if (m_SpriteRenderer.color != Color.white)
            {
                m_SpriteRenderer.color = Color.white;
            }
            else
            {
                m_SpriteRenderer.color = Color.red;
            }
        }

        public void ResetBlink()
        {
            m_SpriteRenderer.color = Color.white;
        }
    }
}
