using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//an abstract class all of our game state classes inherit from
//all of the functions in this class are guaranteed to be called
//in the primary logic flow
//-- Nick

using SFML;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

using InvasionGrid.SystemMessages;

namespace InvasionGrid.GameStates
{
    abstract class GameState
    {
        abstract public void Update(Time timeElapsed);              //a method to handle updating the state of our....state :/
        abstract public void Draw(RenderWindow window);
        abstract public SystemMessage HandleKeyInput(Window window, KeyEventArgs e);
        abstract public SystemMessage HandleMouseMoveInput(Window window, MouseMoveEventArgs e);
        abstract public SystemMessage HandleMousePressInput(Window window, MouseButtonEventArgs e);
        abstract public SystemMessage HandleMouseReleaseInput(Window window, MouseButtonEventArgs e);
        abstract public SystemMessage HandleMouseWheelInput(Window window, MouseWheelEventArgs e);


    }
}
