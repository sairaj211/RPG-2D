using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player m_Player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        m_Player.AnimationTrigger();
    }
}
