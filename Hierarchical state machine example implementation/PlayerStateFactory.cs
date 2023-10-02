/**
The PlayerStateFactory class is a factory for creating different player states.
This class is responsible for creating new state instances for the player state machine.
@author: Sonny Selten
@date 19 april 2023
**/

namespace Multiplayer.PlayerController
{
    public class PlayerStateFactory
    {
        private PlayerStateMachine _context;

        /// Constructor for PlayerStateFactory. Initializes the factory with a reference to the PlayerStateMachine.
        /// <param name="currentContext">The PlayerStateMachine instance that holds the state machine's context.
        public PlayerStateFactory(PlayerStateMachine currentContext)
        {
            _context = currentContext; //when creating a PlayerStateFactory, pass a reference to the PlayerStateMachine
        }

        public PlayerBaseState Idle()
        {
            return new PlayerIdleState(_context, this);
        }

        public PlayerBaseState Run()
        {
            return new PlayerRunState(_context, this);
        }

        public PlayerBaseState Jump()
        {
            return new PlayerJumpState(_context, this);
        }
        public PlayerBaseState Grounded()
        {
            return new PlayerGroundedState(_context, this);
        }

        public PlayerBaseState Dash()
        {
            return new PlayerDashState(_context, this);
        }

        public PlayerBaseState swing()
        {
            return new PlayerSwingState(_context, this);
        }
        public PlayerBaseState Vault()
        {
            return new PlayerVaultState(_context, this);
        }
    }
}