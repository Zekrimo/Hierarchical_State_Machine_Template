using UnityEngine;

namespace Multiplayer.PlayerController
{
    public class PlayerGroundedState : PlayerBaseState
    {
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");

        public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) //passed arguments to base state constructor
        {
            IsRootState = true;
            InitializeSubState();
        }

        //defines the onEnter function of a state machine
        public override void EnterState()
        {
            if (Ctx.ShowCurrentStateInLog) Debug.Log("current player super state: GROUNDED");
            Ctx.Animator.SetBool(IsGrounded, true);
            HandleResistance();
        }

        //defines the onDo function of a state machine
        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        public override void FixedUpdateStates()
        {
            MovePlayer();
        }

        //defines the onExit function of a state machine
        public override void ExitState()
        {
            Ctx.Animator.SetBool(IsGrounded, false);
            Ctx.onStopRunning?.Invoke();
        }

        //Condition to check when to switch to a new state 
        public override void CheckSwitchStates()
        {
            if (Ctx.IsJumpPressed || !Ctx.IsGrounded)
            {
                SwitchState(Factory.Jump());
            }
        }

        //defines which state to intialise when switching substates    
        public override void InitializeSubState()
        {
            if (Ctx.IsDashPressed)
            {
                SetSubState(Factory.Dash());
            }
            else if (Ctx.IsGrapplePressed)
            {
                SetSubState(Factory.swing());
            }
            else if (Ctx.Rigidbody.velocity.x < 0.1 || Ctx.Rigidbody.velocity.z < 0.1)
            {
                SetSubState(Factory.Idle());
            }
            else if (Ctx.Rigidbody.velocity != Vector3.zero)
            {
                SetSubState(Factory.Run());
            }
        }

        private void HandleResistance()
        {
            // Handle the resistance of the player.
            Ctx.Rigidbody.drag = Ctx.DragResistance;
        }

        private void MovePlayer()
        {
            Transform position = Ctx.transform;
            Vector3 movement = position.forward * (Ctx.MovementInput.y * Ctx.MoveSpeed) +
                               position.right * (Ctx.MovementInput.x * Ctx.MoveSpeed);
            Ctx.Rigidbody.AddForce(movement, ForceMode.Force);
        }
    }
}