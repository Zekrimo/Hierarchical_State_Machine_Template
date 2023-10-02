/**
The PlayerStateFactory class is a factory for creating different player states.
This class is responsible for creating new state instances for the player state machine.
@author: Sonny Selten
@date 19 april 2023
**/

public class StateFactory
{
    private StateMachine _context;

    /// Constructor for PlayerStateFactory. Initializes the factory with a reference to the StateMachine.
    /// <param name="currentContext">The PlayerStateMachine instance that holds the state machine's context.
    public StateFactory(StateMachine currentContext)
    {
        _context = currentContext; //when creating a StateFactory, pass a reference to the StateMachine
    }

    public BaseState SuperState()
    {
        return new SuperState(_context, this);
    }

    public BaseState SubState()
    {
        return new SubState(_context, this);
    }

}