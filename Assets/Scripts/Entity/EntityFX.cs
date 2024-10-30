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

        [SerializeField] private float m_EffectsTicksPerSeconds;
        private int m_CurrentColorIndex = 0; // Track the current color index

        [SerializeField] private Color[] m_IgniteColors;
        [SerializeField] private Color m_ChillColor;
        [SerializeField] private Color[] m_ShockColors;
        
        private Coroutine m_CurrentEffectCoroutine;
        
        private void Start()
        {
            m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            m_OriginalMaterial = m_SpriteRenderer.material;
        }

        public IEnumerator FlashFX()
        {
            m_SpriteRenderer.material = m_HitMaterial;
            Color currentColor = m_SpriteRenderer.color; 
            m_SpriteRenderer.color = Color.white;

            yield return new WaitForSeconds(m_FlashTime);

            m_SpriteRenderer.color = currentColor;
            m_SpriteRenderer.material = m_OriginalMaterial;
        }

        public void BlinkEffect()
        {
            m_SpriteRenderer.color = m_SpriteRenderer.color != Color.white ? Color.white : Color.red;
        }
        
        public void ResetBlink()
        {
            if (m_CurrentEffectCoroutine != null)
            {
                StopCoroutine(m_CurrentEffectCoroutine);
                m_CurrentEffectCoroutine = null;
            }
            m_SpriteRenderer.color = Color.white;
        }

        public void IgniteFx(float duration)
        {
            StartEffect(IgniteColor, duration);
        }

        public void ChillFx(float duration)
        {
            ResetBlink(); // Ensure to reset previous effects
            m_SpriteRenderer.color = m_ChillColor;
            Invoke(nameof(ResetBlink), duration);
        }

        public void ShockFx(float duration)
        {
            StartEffect(ShockColor, duration);
        }
        
        private void StartEffect(System.Action _effectMethod, float _duration)
        {
            ResetBlink(); // Ensure to reset previous effects
            m_CurrentEffectCoroutine = StartCoroutine(InvokeRepeatingEffect(_effectMethod, _duration));
        }

        private IEnumerator InvokeRepeatingEffect(System.Action _effectMethod, float _duration)
        {
            float elapsed = 0f;
            while (elapsed < _duration)
            {
                _effectMethod.Invoke();
                yield return new WaitForSeconds(m_EffectsTicksPerSeconds);
                elapsed += m_EffectsTicksPerSeconds;
            }
            ResetBlink();
        }
    
        public void MakeTransparent(bool _isTransparent)
        {
            if (_isTransparent)
            {
                m_SpriteRenderer.color = Color.clear;
            }
            else
            {
                m_SpriteRenderer.color = Color.white;
            }
        }

        private void IgniteColor()
        {
            UpdateColor(m_IgniteColors);
        }

        private void ShockColor()
        {
            UpdateColor(m_ShockColors);
        }

        private void UpdateColor(Color[] colors)
        {
            m_SpriteRenderer.color = colors[m_CurrentColorIndex];
            m_CurrentColorIndex = (m_CurrentColorIndex + 1) % colors.Length; 
        }
    }
}
