//TODO: STATE NOT YET USED, NOT YET USED
//[Obsolete]

using UnityEngine;

namespace Multiplayer.PlayerController
{
    public class PlayerDashState : PlayerBaseState
    {
        //constructor
        public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) //passed arguments to base state constructor
        {

        }
        //defines the onEnter function of a state machine
        public override void EnterState()
        {
            if (Ctx.ShowCurrentStateInLog) Debug.Log("current playerstate: DASH");
            Ctx.IsDashCompleted = false;
            
            Ctx.Animator.SetBool("IsDashing", true);
            

        }

        //defines the onDo function of a state machine
        public override void UpdateState()
        {
            Ctx.IsDashCompleted = true;
            CheckSwitchStates();

        }

        //defines the onExit function of a state machine
        public override void ExitState()
        {
            Debug.Log("Dash exit called");
            Ctx.IsDashPressed = false;
            Ctx.Animator.SetBool("IsDashing", false);
        }

        //Condition to check when to switch to a new state 
        public override void CheckSwitchStates()
        {
            if (Ctx.IsDashCompleted == true && Ctx.IsGrapplePressed)
            {
                SwitchState(Factory.swing());
            }
            // else if (Ctx.IsDashCompleted == true && (Ctx.Rigidbody.velocity.x > 0.1 || Ctx.Rigidbody.velocity.x < -0.1 || Ctx.Rigidbody.velocity.z > 0.1 || Ctx.Rigidbody.velocity.z < -0.1))
            // {
            //     SwitchState(Factory.Run());
            // } 
            else if (Ctx.IsGrapplePressed)
            {
                SwitchState(Factory.swing());
            }
            else if (Ctx.IsDashCompleted == true)
            {
                SwitchState(Factory.Idle());
            } 
        }

        //defines which state to intialise when switching substates    
        public override void InitializeSubState()
        {
            //TODO  
        }

        ////    Member Functions

    }
}
