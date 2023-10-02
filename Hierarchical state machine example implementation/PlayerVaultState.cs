//TODO: STATE NOT YET USED, NOT YET USED
//[Obsolete]

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer.PlayerController
{
    public class PlayerVaultState : PlayerBaseState
    {
        //constructor
        public PlayerVaultState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) //passed arguments to base state constructor
        {

        }

        //defines the onEnter function of a state machine
        public override void EnterState()
        {
            if (Ctx.ShowCurrentStateInLog) Debug.Log("current playerstate: VAULTING");
        }

        //defines the onDo function of a state machine
        public override void UpdateState()
        {
            CheckSwitchStates();
        }

        //defines the onExit function of a state machine
        public override void ExitState()
        {
            //TODO
        }

        //Condition to check when to switch to a new state 
        public override void CheckSwitchStates()
        {
            //TODO
        }

        //defines which state to intialise when switching substates    
        public override void InitializeSubState()
        {
            //TODO
        }
    }
}