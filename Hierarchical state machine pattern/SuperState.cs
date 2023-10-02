public class SuperState : BaseState
{

    public SuperState(ConcreteStateMachine currentContext, StateFactory StateFactory)
        : base(currentContext, StateFactory) //passed arguments to base state constructor
    {
        IsRootState = true; //defines if this state is a root state
        InitializeSubState();
    }

    //defines the onEnter function of a state machine
    public override void EnterState()
    {
        if (Ctx.ShowCurrentStateInLog) Debug.Log("current super state: SUPER_STATE_NAME ");

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
        if (/*CONDITION TO CHANGE TO NEXT SUPER STATE*/)
        {
            SwitchState(Factory.SuperState());
        }
    }

    //defines which state to intialise when switching SuperStates    
    public override void InitializeSubState()
    {
        if (/* CONDITION TO INITIALIZE A SUBSTATE*/)
        {
            SetSubState(Factory.SubState());
        }
        else if (/* CONDITION TO INITIALIZE ANOTHER SUBSTATE*/)
        {
            SetSubState(Factory.SubState());
        }
    }

    //Function to be executed only in this super state go here

}