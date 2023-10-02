/**
The PlayerIdleState class represents the idle state of the player in a finite state machine.
This state is activated when the player is not moving.
@note: also see PlayerBaseState.cs
@author: Sonny Selten
@date 19 april 2023
**/

using UnityEngine;

namespace Multiplayer.PlayerController
{
    public class PlayerIdleState : PlayerBaseState
    {
        /// Constructor for PlayerIdleState. Initializes base state with provided context and factory.
        /// <param name="currentContext">The PlayerStateMachine instance that holds the state machine's context.</param>
        /// <param name="playerStateFactory">The PlayerStateFactory instance used to create new states.</param>
        public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) //passed arguments to base state constructor
        {

        }
        //defines the onEnter function of a state machine
        public override void EnterState()
        {
            if (Ctx.ShowCurrentStateInLog) Debug.Log("current playerstate: IDLE");
            Ctx.Animator.SetBool("IsIdle", true);

        }

        //defines the onDo function of a state machine
        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        //defines the onExit function of a state machine
        public override void ExitState()
        {
            Ctx.Animator.SetBool("IsIdle", false);
        }

        //Condition to check when to switch to a new state 
        public override void CheckSwitchStates()
        {
            if(Ctx.IsDashPressed)
            {
                SwitchState(Factory.Dash());
            }
            if (Ctx.IsGrapplePressed)
            {
                SwitchState(Factory.swing());
            }
            else if (Ctx.Rigidbody.velocity.x > 0.1 || Ctx.Rigidbody.velocity.x < -0.1 || Ctx.Rigidbody.velocity.z > 0.1 || Ctx.Rigidbody.velocity.z < -0.1)
            {
                SwitchState(Factory.Run());
            }
        }

        //defines which state to intialise when switching substates    
        public override void InitializeSubState()
        {
            //TODO
        }
    }
}
