using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

using InvasionGrid.SystemMessages;

//HardcoreModeState:
//This state  class isd responsible for all
//of the logic regarding the hardcore mode of this
//game
namespace InvasionGrid.GameStates
{
    class HardcoreModeState : GameState
    {
        private Font font;
        private Text title;

        public HardcoreModeState()
        {
            font = new Font("sansation.ttf");

            title = new Text("-HARDCORE MODE-", font);
            title.Position = new Vector2f(50.0f, 10.0f);
            title.CharacterSize = 40;
        }

        override public void Update(Time timeElapsed)
        {

        }

        override public void Draw(RenderWindow window)
        {
            window.Draw(title);
        }

        override public SystemMessage HandleKeyInput(Window window, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                return new PopStateMessage(this);
            }

            return null;
        }

        override public SystemMessage HandleMouseMoveInput(Window window, MouseMoveEventArgs e) { return null; }
        override public SystemMessage HandleMousePressInput(Window window, MouseButtonEventArgs e) { return null; }
        override public SystemMessage HandleMouseReleaseInput(Window window, MouseButtonEventArgs e) { return null; }
        override public SystemMessage HandleMouseWheelInput(Window window, MouseWheelEventArgs e) { return null; }
    }
}
