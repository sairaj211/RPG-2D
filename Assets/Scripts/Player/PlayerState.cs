using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine m_PlayerStateMachine;
    protected Player m_Player;
    protected Vector2 m_InputMovement;
    protected Rigidbody2D m_Rigidbody2D;
    protected Animator m_Animator;

    private int m_AnimationHash;
    protected float m_StateTimer;
    protected bool m_TriggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, int _animHash)
    {
        m_Player = _player;
        m_PlayerStateMachine = _playerStateMachine;
        m_AnimationHash = _animHash;
    }

    public virtual void Enter()
    {
        m_Animator = m_Player.GetAnimator();
        m_Animator.SetBool(m_AnimationHash, true);
        m_Rigidbody2D = m_Player.GetRigidbody();
        m_TriggerCalled = false;
    }

    public virtual void Update()
    {
        m_StateTimer -= Time.deltaTime;
        PlayerInput();
  //     m_Animator.SetFloat(PlayerStatesAnimationHash.JUMP_VELOCITY, m_Rigidbody2D.velocityY);
        
    //    Debug.Log(m_PlayerStateMachine.m_CurrentState.ToString());
    }
 
    public virtual void Exit()
    {
        m_Animator.SetBool(m_AnimationHash, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        m_TriggerCalled = true;
    }
    
    private void PlayerInput()
    {
        m_InputMovement.x = Input.GetAxisRaw("Horizontal");//m_Player.m_PlayerControls.Movement.Move.ReadValue<Vector2>().x;
        m_InputMovement.y = Input.GetAxisRaw("Vertical");  //m_Player.m_PlayerControls.Movement.Move.ReadValue<Vector2>().y;

        m_InputMovement.Normalize();
    }
}
