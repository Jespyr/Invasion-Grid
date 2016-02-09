using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InvasionGrid.GameStates;

//PopStateMessage
//This message tells the primary class to pop
//the current state off the stack, after calling
//the appropriate functions first (for things
//like retrieving the player's score, etc
namespace InvasionGrid.SystemMessages
{
    class PopStateMessage : SystemMessage
    {
        private GameState curState;

        public PopStateMessage(GameState curState)
        {
            if(curState == null)
                throw new NullReferenceException("Cannot Make PopStateMessage without valid current state");

            this.curState = curState;
        }

        override public void PrintMessage()
        {
            Console.WriteLine("SystemMessage: PopState. CurrentState: " + curState.ToString());
;        }
    }
}
