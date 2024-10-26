using System;
using UnityEngine;

public class ThunderAnimationTrigger : MonoBehaviour
{
    private ThunderStrikeController m_Controller;

    private void Start()
    {
        m_Controller = GetComponentInParent<ThunderStrikeController>();
    }

    private void AnimationTrigger()
    {
        m_Controller.DamageAndSelfDestroy();
    }
}
