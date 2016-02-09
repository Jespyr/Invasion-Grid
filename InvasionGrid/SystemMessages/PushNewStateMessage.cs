using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InvasionGrid.GameStates;

//PushStateMessage:
//This message tells the primary class to 
//push a new state onto the stack. This will
//probably only be returned by the menu, but
//its good to be consistent.
//Note that the new state needs to be properly
//initialized by the current state
namespace InvasionGrid.SystemMessages
{
    class PushNewStateMessage : SystemMessage
    {
        private GameState targetState;

        public PushNewStateMessage(GameState newState)
        {
            //make sure the state is valid
            if (newState == null)
                throw new NullReferenceException("Cannot Make PushNewStateMessage without valid target state");

            targetState = newState;
        }

        override public void PrintMessage()
        {
            Console.WriteLine("SystemMessage: Push new state: " + targetState.ToString());
        }

        public GameState getState()
        {
            return targetState;
        }
    }
}
