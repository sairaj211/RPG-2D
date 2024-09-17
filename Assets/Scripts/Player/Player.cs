using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Player : Entity
    {
        [Header("Settings")] 
        public float m_MoveSpeed;
        public float m_JumpVelocity;
        public float m_FallSpeed;
        public float m_CounterAttackDuration = 0.2f;
        public float m_SwordReturnImpact;
        public bool m_IsBusy { private set; get; }
        [Header("Dash")]
        public float m_DashSpeed;
        public float m_DashDuration;
        public float m_DashDirection { private set; get; }
        private float m_DashTimer = 0f;
        [Header("WallJump")] 
        public float m_WallJumpVelocity;
        public float m_WallJumpDuration;
        [Header("Attack Details")] 
        public Vector2[] m_AttackMovements;
        
        public GameObject m_Sword { get; private set; }

    
        #region STATES
        public PlayerStateMachine m_PlayerStateMachine { get; private set; }
        public PlayerIdleState m_IdleState { get; private set; }
        public PlayerMoveState m_MoveState { get; private set; }
        public PlayerAirState m_AirState { get; private set; }
        public PlayerJumpState m_JumpState { get; private set; }
        public PlayerDashState m_DashState { get; private set; }
        public PlayerWallSlideState m_WallSlideState { get; private set; }
        public PlayerWallJumpState m_WallJumpState { get; private set; }
        public PlayerPrimaryAttackState m_PrimaryAttackState { get; private set; }
        public PlayerCounterAttackState m_PlayerCounterAttackState { get; private set; }
        public PlayerAimSwordState m_PlayerAimSwordState { get; private set; }
        public PlayerCatchSwordState m_PlayerCatchSwordState { get; private set; }
        #endregion

        #region PlayerControlVariables
        private Vector2 m_Velocity;
        public PlayerControls m_PlayerControls;
        #endregion

        public SkillManager m_SkillManager { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            m_PlayerStateMachine = new PlayerStateMachine();
            m_PlayerControls = new PlayerControls();
            m_PlayerControls.Enable();
            m_PlayerControls.Movement.Dash.performed += DashOnPerformed;
        
            m_IdleState = new PlayerIdleState(this, m_PlayerStateMachine, EntityStatesAnimationHash.IDLE);
            m_MoveState = new PlayerMoveState(this, m_PlayerStateMachine, EntityStatesAnimationHash.MOVE);
            m_JumpState = new PlayerJumpState(this, m_PlayerStateMachine, EntityStatesAnimationHash.JUMP);
            m_AirState  = new PlayerAirState(this, m_PlayerStateMachine, EntityStatesAnimationHash.JUMP);
            m_DashState  = new PlayerDashState(this, m_PlayerStateMachine, EntityStatesAnimationHash.DASH);
            m_WallSlideState  = new PlayerWallSlideState(this, m_PlayerStateMachine, EntityStatesAnimationHash.WALLSLIDE);
            m_WallJumpState  = new PlayerWallJumpState(this, m_PlayerStateMachine, EntityStatesAnimationHash.JUMP);
            m_PrimaryAttackState = new PlayerPrimaryAttackState(this, m_PlayerStateMachine, EntityStatesAnimationHash.ATTACK);
            m_PlayerCounterAttackState = new PlayerCounterAttackState(this, m_PlayerStateMachine, EntityStatesAnimationHash.COUNTERATTACK);
            m_PlayerAimSwordState = new PlayerAimSwordState(this, m_PlayerStateMachine, EntityStatesAnimationHash.SWORD_AIM);
            m_PlayerCatchSwordState = new PlayerCatchSwordState(this, m_PlayerStateMachine, EntityStatesAnimationHash.SWORD_CATCH);
        }

        private void OnDestroy()
        {
            m_PlayerControls.Movement.Dash.performed -= DashOnPerformed;
        }
    
        protected override void Start()
        {
            base.Start();
            base.SetSpeed(m_MoveSpeed);
            m_PlayerStateMachine.Initialize(m_IdleState);
            m_SkillManager = SkillManager.Instance;
        }

        protected override void Update()
        {
            base.Update();

            m_PlayerStateMachine.m_CurrentState.Update();
            m_DashTimer -= Time.deltaTime;
        }

        public IEnumerator BusyFor(float _seconds)
        {
            m_IsBusy = true;

            yield return new WaitForSeconds(_seconds);

            m_IsBusy = false;
        }
    
        public void AnimationTrigger() => m_PlayerStateMachine.m_CurrentState.AnimationFinishTrigger();

        private void DashOnPerformed(InputAction.CallbackContext _obj)
        {
            if (IsWallDetected())
            {
                return;
            }
        
            if (SkillManager.Instance.m_DashSkill.CanUseSkill())
            {
                m_DashDirection = m_PlayerControls.Movement.Move.ReadValue<Vector2>().x;

                if (m_DashDirection == 0f)
                {
                    m_DashDirection = m_FacingDireciton;
                }
            
                m_PlayerStateMachine.ChangeState(m_DashState);
            }
        }

        public void AssignNewSword(GameObject _sword)
        {
            m_Sword = _sword;
        }

        public void CatchSword()
        {
            m_PlayerStateMachine.ChangeState(m_PlayerCatchSwordState);
            Destroy(m_Sword);
        }
    }
}
