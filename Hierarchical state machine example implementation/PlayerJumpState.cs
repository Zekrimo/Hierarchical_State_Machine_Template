/**
 The PlayerJumpState class represents the jumping state of the player in a finite state machine.
 This state is activated when the player initiates a jump.
@note: also see PlayerBaseState.cs
@author: Sonny Selten
@date 19 april 2023
**/

using UnityEngine;

namespace Multiplayer.PlayerController
{
    public class PlayerJumpState : PlayerBaseState
    {
        /// Constructor for PlayerJumpState. Initializes base state with provided context and factory.
        /// <param name="currentContext">The PlayerStateMachine instance that holds the state machine's context.</param>
        /// <param name="playerStateFactory">The PlayerStateFactory instance used to create new states.</param>
        public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) //passed arguments to base state constructor
        {
            IsRootState = true;
            InitializeSubState();
        }

        //defines the onEnter function of a state machine
        public override void EnterState()
        {
            if (Ctx.ShowCurrentStateInLog) Debug.Log("current player super state: JUMPING");
            Ctx.Animator.SetBool("IsJumping", true);
            HandleResistance();
            if (Ctx.IsJumpPressed)
            {
                Ctx.Animator.SetBool("IsJumping", true);
                HandleJump();
            }
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
            Ctx.Animator.SetBool("IsJumping", false);
        }

        //Condition to check when to switch to a new state 
        public override void CheckSwitchStates()
        {
            if (Ctx.IsGrounded)
            {
                SwitchState(Factory.Grounded());
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
            else if (Ctx.Rigidbody.velocity.x > 0.1 || Ctx.Rigidbody.velocity.z > 0.1)
            {
                SetSubState(Factory.Run());
            }
            else
            {
                SetSubState(Factory.Idle());
            }
        }

        //-------------------------------------------------------------------//

        //logic functions
        private void HandleJump()
        {
            Debug.Log("player Jumped");
            // Reset y velocity
            Vector3 velocity = Ctx.Rigidbody.velocity;

            // AddForce can bundle up for some reason, so manually setting the velocity solves the jumping problem
            velocity = new Vector3(velocity.x, Ctx.JumpHeight, velocity.z);
            Ctx.Rigidbody.velocity = velocity;
        }

        private void HandleResistance()
        {
            // Handle the resistance of the player.
            Ctx.Rigidbody.drag = Ctx.DragResistanceInAir;
        }

        private void MovePlayer()
        {
            Transform position = Ctx.transform;
            Vector3 movement = Vector3.zero;

            var forwardVelocity = Vector3.Dot(Ctx.Rigidbody.velocity, position.forward);
            var sideVelocity = Vector3.Dot(Ctx.Rigidbody.velocity, position.right);
            
            if (forwardVelocity > Ctx.MaxSpeedForStrafing && Ctx.MovementInput.y > 0)
            {
                // Debug.Log("Cannot add forward force: "+forwardVelocity);
            }
            else if (forwardVelocity < -Ctx.MaxSpeedForStrafing && Ctx.MovementInput.y < 0)
            {
                // Debug.Log("Cannot add backward force: "+forwardVelocity);
            }
            else
            {
                movement += position.forward * (Ctx.MovementInput.y * Ctx.StrafeForce);
            }

            if (sideVelocity > Ctx.MaxSpeedForStrafing && Ctx.MovementInput.y > 0)
            {
                // Debug.Log("Cannot add right force: "+sideVelocity);
            }
            else if (sideVelocity < -Ctx.MaxSpeedForStrafing && Ctx.MovementInput.y < 0)
            {
                // Debug.Log("Cannot add left force: "+sideVelocity);
            }
            else
            {
                movement += position.right * (Ctx.MovementInput.x * Ctx.StrafeForce);
            }
            Ctx.Rigidbody.AddForce(movement, ForceMode.Force);
        }
    }
}