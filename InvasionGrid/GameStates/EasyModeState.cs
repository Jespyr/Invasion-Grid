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
        private EasyGraph graph;

        EasyGraphNode center;
        EasyGraphNode north;
        EasyGraphNode south;
        EasyGraphNode east;
        EasyGraphNode west;
        EasyGraphNode wuuu;



        public EasyModeState()
        {
            font = new Font("sansation.ttf");

            title = new Text("Welcome to easy mode, scrub", font);
            title.Position = new Vector2f(50.0f, 10.0f);
            title.CharacterSize = 40;

            graph = new EasyGraph();
            CreateLevelGrid(0);

        }

        override public void Update(Time timeElapsed)
        {
            
        }

        override public void Draw(RenderWindow window)
        {
            window.Draw(title);
            graph.Draw(window);
        }

        override public SystemMessage HandleKeyInput(Window window, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                return new PopStateMessage(this);
            }

            return null;
        }

        //A function for creating a 
        private void CreateLevelGrid(int level)
        {
            graph.Clear();

            Console.WriteLine("Building graph for level " + level + "...");
            if (level == 0)
            {
                center = graph.MakeNode(new Vector2f(400.0f, 300.0f));
                north = graph.MakeNode(new Vector2f(400.0f, 200.0f));
                south = graph.MakeNode(new Vector2f(400.0f, 400.0f));
                east = graph.MakeNode(new Vector2f(500.0f, 300.0f));
                west = graph.MakeNode(new Vector2f(300.0f, 300.0f));
                wuuu = graph.MakeNode(new Vector2f(500.0f, 500.0f));

                graph.MakeEdge(center, north, 4);
                graph.MakeEdge(center, south, 4);
                graph.MakeEdge(center, west, 4);
                graph.MakeEdge(center, east, 4);
                graph.MakeEdge(north, east, 10);
                graph.MakeEdge(south, west, 20);
                graph.MakeEdge(west, north, 20);
                graph.MakeEdge(east, south, 50);
                graph.MakeEdge(north, wuuu, 900);
            }
            else if (level == 1)
            {

            }
            else if (level == 2)
            {
            }
            else
            {
                Console.WriteLine("Invalid level, cannot build graph");
            }

            Console.WriteLine("Done!");
            
        }
    }
}
