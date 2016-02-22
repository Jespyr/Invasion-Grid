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

//This State Handles all of the operations necessary
//for the main menu: moving to easy mode, moving
//to hardcore mode, checking the high scores
//and quitting the game
namespace InvasionGrid.GameStates
{
    class MenuState : GameState
    {

        private Font font;          //the font our text uses
        private Text title;         //our title
        private Text[] options;     //our options render object
        private Text pointer;       //our pointer render object
        private int selected;       //the menu option we have selected

        public MenuState()
        {
            //initializing the menu object we have selected
            //this is honesty unnecessary, but i like to make things explicit
            selected = 0;

            //our font object our text uses
            //(note: use a different font, this one is pulled directly from the SFML.net examples)
            font = new Font("sansation.ttf");

            //the title of our menu
            title = new Text("INVASION GRID", font);
            title.Position = new Vector2f(50.0f, 10.0f);
            title.CharacterSize = 40;

            //setup for our options text
            options = new Text[4];

            options[0] = new Text("Easy Mode", font);
            options[1] = new Text("HARDCORE MODE", font);
            options[2] = new Text("High Scores", font);
            options[3] = new Text("Quit", font);

            for (int i = 0; i < 4; i++)
            {
                options[i].Position = new Vector2f(50.0f, 100.0f * (i + 1));
                options[i].Color = Color.Red;
            }

            //setup for our pointer
            pointer = new Text(">", font);
            pointer.Position = new Vector2f(30.0f, 100.0f);
            pointer.Color = Color.White;
        }

        override public void Update(Time timeElapsed)
        {

        }

        override public void Draw(RenderWindow window)
        {
            //code to draw our text objects on the screen
            window.Draw(title);

            for (int i = 0; i < 4; i++)
            {
                window.Draw(options[i]);
            }

            window.Draw(pointer);
        }
        
        override public SystemMessage HandleKeyInput(Window window, KeyEventArgs e)
        {
            //check whi h key has been pressed and react accordingly
            if(e.Code == Keyboard.Key.Return)
            {
                //act differently depending on which menu object we have selected
                if (selected == 0)//easy mode
                {
                    return new PushNewStateMessage(new EasyModeState());
                }
                else if (selected == 1)
                {
                    return new PushNewStateMessage(new HardcoreModeState());
                }
                else if (selected == 2)
                {
                    return new PushNewStateMessage(new ScoreBoardState());
                }
                else if (selected == 3)//quit the game
                {
                    return new QuitMessage();
                }
            }
            else if(e.Code == Keyboard.Key.Down)
            {
                if (selected < 3)
                {
                    selected++;
                }
                else
                {
                    selected = 0;
                }
                pointer.Position = new Vector2f(30.0f, 100.0f * (selected + 1));
            }
            else if (e.Code == Keyboard.Key.Up)
            {
                if (selected > 0)
                {
                    selected--;
                }
                else
                {
                    selected = 3;
                }
                pointer.Position = new Vector2f(30.0f, 100.0f * (selected + 1));
            }
            return null;
        }

        
        override public SystemMessage HandleMouseMoveInput(Window window, MouseMoveEventArgs e) { return null; }
        override public SystemMessage HandleMousePressInput(Window window, MouseButtonEventArgs e) { return null; }
        override public SystemMessage HandleMouseReleaseInput(Window window, MouseButtonEventArgs e) { return null; }
        override public SystemMessage HandleMouseWheelInput(Window window, MouseWheelEventArgs e) { return null; }
    
    }
}
