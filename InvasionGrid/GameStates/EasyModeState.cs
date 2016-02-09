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


//EasyModeState
//for the moment, easy mode will be used to demonstrate
//which graphical abilities we have available to us
//text, lines, circles, rectangles, etc
//This state is intended to handle all of the 
//logic dedicated to the operation of the easy
//mode version of this game
namespace InvasionGrid.GameStates
{
    class EasyModeState : GameState
    {

        private Font font;
        private Text title;



        public EasyModeState()
        {
            font = new Font("sansation.ttf");

            title = new Text("Welcome to easy mode, scrub", font);
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
    }
}
