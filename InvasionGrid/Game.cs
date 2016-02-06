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

namespace InvasionGrid
{
    class Game
    {
        //SFML primary objects
        RenderWindow window;
        public Game()
        {
            window = new RenderWindow(new VideoMode(800, 600), "Invasion Grid");
            window.Closed += new EventHandler(OnClosed);
            window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);

        }

        public void Run()
        {
            //initialization
            window.SetActive();

            //loop
            while(window.IsOpen)
            {
                window.DispatchEvents();

                //handle logic here

                window.Clear();

                //draw objects here

                window.Display();
            }
        }

        public void OnClosed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Close();
            //shutdown code
        }

        public void OnKeyPressed(object sender, KeyEventArgs e)
        {
            Window window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
                window.Close();

        }

        
    }
}
