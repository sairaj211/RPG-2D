using UnityEngine;

public class PlayerStatesAnimationHash 
{
    public static readonly int IDLE = Animator.StringToHash("Idle");
    public static readonly int MOVE = Animator.StringToHash("Move");
    public static readonly int JUMP = Animator.StringToHash("Jump");
    public static readonly int DASH = Animator.StringToHash("Dash");
    public static readonly int WALLSLIDE = Animator.StringToHash("WallSlide");
    public static readonly int JUMP_VELOCITY = Animator.StringToHash("yVelocity");
    public static readonly int ATTACK = Animator.StringToHash("Attack");
    public static readonly int COMBO_COUNTER = Animator.StringToHash("ComboCounter");
    
    //Enemy state
    public static readonly int BATTLE = Animator.StringToHash("BATTLE");
    
}
