using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//BIT 265 Winter 2016
//Invasion Grid
//Game.cs
//this class handles the overarching program flow,
//as well as library loading and management (SFML)

using SFML;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

using InvasionGrid.GameStates;
using InvasionGrid.SystemMessages;

namespace InvasionGrid
{
    class Game
    {
        //SFML primary objects
        RenderWindow window;
        Stack<GameState> GameStates;

        public Game()
        {
            window = new RenderWindow(new VideoMode(800, 600), "Invasion Grid");
            window.Closed += new EventHandler(OnClosed);
            window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

            //set us up with the first game state (the menu)
            GameStates = new Stack<GameState>();
            GameStates.Push(new MenuState());

        }

        public void Run()
        {
            //initialization
            window.SetActive();

            Clock clock = new Clock();
            clock.Restart();
            //loop
            while(window.IsOpen)
            {
                //make sure windows events are handled
                window.DispatchEvents();

                //handle logic here
                Time elapsed = clock.Restart(); //elapsed is the amount of time elapsed since the last loop
                GameStates.Peek().Update(elapsed);

                //clear the window
                window.Clear();

                //draw objects here
                GameStates.Peek().Draw(window);

                //draw the object we placed on our frame
                window.Display();
            }

            //clean up
            GameStates.Clear();
                
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Close();
            //shutdown code
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            SystemMessage curMessage;
            curMessage = GameStates.Peek().HandleKeyInput((Window)sender, e); //delegate the key handling to our current state

            HandleSystemMessage(curMessage);

        }

        private void HandleSystemMessage(SystemMessage curMessage)
        {
            //if the iput handler has nothing to tell us, we're done
            if (curMessage == null)
                return;

            //otherwise handle the messages we get back
            if (curMessage is QuitMessage)
            {
                Console.WriteLine("Quitting!");
                window.Close();
            }
            else if (curMessage is PushNewStateMessage)
            {
                //new state creation is handled by the current state
                GameStates.Push(((PushNewStateMessage)curMessage).getState());
            }
            else if (curMessage is PopStateMessage)
            {
                //retrieve data from the current state, then pop
                //check for the current state type
                //and do something different with the top state depending on what it is
                if (GameStates.Peek() is EasyModeState)
                {
                    Console.WriteLine("Moving from easy mode to menu");
                }
                else if (GameStates.Peek() is HardcoreModeState)
                {
                    Console.WriteLine("Moving from HARDCORE mode to menu");
                }
                else if (GameStates.Peek() is ScoreBoardState)
                {
                    Console.WriteLine("Moving from scoreboard to menu");
                }
                //....etc
                GameStates.Pop();
            }
        }
        
    }
}
