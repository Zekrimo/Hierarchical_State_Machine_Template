/**
The PlayerIdleState class represents the idle state of the player in a finite state machine.
This state is activated when the player is not moving.
@note: also see PlayerBaseState.cs
@author: Sonny Selten
@date 25 april 2023
**/

using UnityEngine;

namespace Multiplayer.PlayerController
{
    public class PlayerSwingState : PlayerBaseState
    {

        float timer;

        public PlayerSwingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) //passed arguments to base state constructor
        {

        }

        //defines the onEnter function of a state machine
        public override void EnterState()
        {
            if (Ctx.ShowCurrentStateInLog) Debug.Log("current playerstate: SWINGING");
            if ((!Ctx.Grapple.IsPlayerSwinging && !Ctx.IsGrappleOut))
            {
                Debug.Log("swing entry");
                Ctx.Grapple.Fire(Ctx.Head.forward);
                Ctx.Animator.SetBool("IsSwinging", true);
                Ctx.IsGrappleOut = true;

            }
        }

        //defines the onDo function of a state machine
        public override void UpdateState()
        {
            CheckSwitchStates();
            if (swingTransitionTimer(3))
            {
                Ctx.Animator.SetBool("IsEndOfSwing", true);
            }
        }

        //defines the onExit function of a state machine
        public override void ExitState()
        {
            if (!Ctx.IsDashPressed && Ctx.IsGrappleOut)
            {
                Debug.Log("swing exit");
                Ctx.Grapple.Release();
                Ctx.Animator.SetBool("IsSwinging", false);
                Ctx.Animator.SetBool("IsEndOfSwing", false);
                resetTimer();
                Ctx.IsGrappleOut = false;
            }
        }

        //Condition to check when to switch to a new state 
        public override void CheckSwitchStates()
        {
            if (!Ctx.IsGrapplePressed)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsDashPressed)
            {
                SwitchState(Factory.Dash());
            }
        }

        //defines which state to intialise when switching substates    
        public override void InitializeSubState()
        {
            //TODO
        }

        private bool swingTransitionTimer(float transitiontime)
        {
            timer += Time.deltaTime;
            if (timer > transitiontime)
            {
                return true;
            }

            return false;
        }

        private void resetTimer()
        {
            timer = 0;
        }
    }
}
