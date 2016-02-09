using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//QuitMessage:
//A SystemMessage telling the game to immediately quit
//Avoid using this in places other than the menu, or when
//critical issues pop up
namespace InvasionGrid.SystemMessages
{
    class QuitMessage : SystemMessage
    {
        public QuitMessage()
        {
        }

        override public void PrintMessage()
        {
            Console.WriteLine("SystemMessage: Quit Game");
        }
    }
}
