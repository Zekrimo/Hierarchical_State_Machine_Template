/**
The PlayerRunState class represents the running state of the player in a finite state machine.
This state is activated when the player is moving.
@note: also see PlayerBaseState.cs
@author: Sonny Selten
@date 19 april 2023
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer.PlayerController
{
public class PlayerRunState : PlayerBaseState
{

    /// Constructor for PlayerRunState. Initializes base state with provided context and factory.
    /// <param name="currentContext">The PlayerStateMachine instance that holds the state machine's context.</param>
    /// <param name="playerStateFactory">The PlayerStateFactory instance used to create new states.</param>
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) //passed arguments to base state constructor
    {

    }
    //defines the onEnter function of a state machine
    public override void EnterState()
    {
        if (Ctx.ShowCurrentStateInLog) Debug.Log("current playerstate: RUNNING");
        Ctx.Animator.SetBool("IsRunning", true);
        
        if(Ctx.IsGrounded)
            Ctx.onStartRunning?.Invoke();
        
    }

    //defines the onDo function of a state machine
    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    //defines the onExit function of a state machine
    public override void ExitState()
    {
        Ctx.Animator.SetBool("IsRunning", false);
        Ctx.onStopRunning?.Invoke();
    }

    //Condition to check when to switch to a new state 
    public override void CheckSwitchStates()
    {
        if (Ctx.IsGrapplePressed)
        {
            SwitchState(Factory.swing());
        }
        else if (Ctx.Rigidbody.velocity.magnitude < 0.1)
        {
            SwitchState(Factory.Idle());
        }
    }

    //defines which state to intialise when switching substates    
    public override void InitializeSubState()
    {
        //TODO
    }

    ////member function

}
}
