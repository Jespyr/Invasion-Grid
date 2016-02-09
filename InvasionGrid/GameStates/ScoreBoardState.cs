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

//ScoreBoardState:
//This state is responsible for all of the logic required
//to display the scoreboard for this
//game
namespace InvasionGrid.GameStates
{
    class ScoreBoardState : GameState
    {
        private Font font;
        private Text title;

        //class specific vars go here
        public ScoreBoardState()
        {
            font = new Font("sansation.ttf");

            title = new Text("Scoreboard", font);
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
