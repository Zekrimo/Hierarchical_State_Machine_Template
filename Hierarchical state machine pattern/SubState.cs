/**
The SubState class represents the idle state of the player in a finite state machine.
This state is activated when the player is not moving.
@note: also see PlayerBaseState.cs
@author: Sonny Selten
@date 19 april 2023
**/

public class SubState : BaseState
{
    /// Constructor for SubState. Initializes base state with provided context and factory.
    /// <param name="currentContext">The PlayerConcreteStateMachine instance that holds the state machine's context.</param>
    /// <param name="StateFactory">The PlayerStateFactory instance used to create new states.</param>
    public SubState(ConcreteStateMachine currentContext, StateFactory StateFactory)
    : base(currentContext, playerStateFactory) //passed arguments to base state constructor
    {

    }
    //defines the onEnter function of a state machine
    public override void EnterState()
    {
        if (Ctx.ShowCurrentStateInLog) Debug.Log("current playerstate: SUB_STATE_NAME");

    }

    //defines the onDo function of a state machine
    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    //defines the onExit function of a state machine
    public override void ExitState()
    {

    }

    //Condition to check when to switch to a new state 
    public override void CheckSwitchStates()
    {
        if (/*CONDITION TO SWITCH TO A SUBSTATE*/)
        {
            SwitchState(Factory.SubState());
        }
        else if (/*CONDITION TO SWITCH TO ANOTHER SUBSTATE*/)
        {
            SwitchState(Factory.SubState());
        }
    }

    //defines which state to intialise when switching substates    
    public override void InitializeSubState()
    {
        //Only implement for superstate
    }
}

