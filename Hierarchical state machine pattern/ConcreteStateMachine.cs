﻿/**
/// ConcreteStateMachine is the context class of the player controller, also known as the player controller.
/// This class inherits from NetworkBehaviour and passes on all NetworkBehaviour functionality to the concrete states.
@author: Sonny Selten
@date 19 april 2023
**/
    public class ConcreteStateMachine
    {
        ///   State Variables
        private BaseState _currentState; //reference to the current state
        private StateFactory _states; //reference to the state factory


        bool _showCurrentStateInLog = true;

        
        //   Update is called once per frame
        private void Update()
        {
            _currentState.UpdateStates();
        }
    }
