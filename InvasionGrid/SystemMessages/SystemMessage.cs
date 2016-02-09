using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//System Message:
//The game engine will have the ability for different game
//states to send messages back to the primary class, so 
//that it will be able to react appropriately, pushing
//new states, popping states, quitting, and whatever else
//becomes necessary. To facilitate this, i've created an
//abstract class all of these messages will derive from
//so all of them can be passed back more easily
//-Nick
namespace InvasionGrid.SystemMessages
{
    abstract class SystemMessage
    {
        public abstract void PrintMessage();
    }
}
