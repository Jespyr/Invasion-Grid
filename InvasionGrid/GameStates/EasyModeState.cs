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

//Nick Carpenetti
//
namespace InvasionGrid.GameStates
{
    class EasyModeState : GameState
    {

        Font font;
        Text instructionText;
        EasyGraph graph;
        int selectedWeight;             
        Text weightText;                    //the display of which weight the next edge will be

        EasyGraphNode enemyNode;
        EasyGraphNode playerNode;
        Text playerIndicator;               //a small piece of text which indicates where the player is
        Text enemyIndicator;                //" " enemy is

        //mouse related variables
        Vector2f oldCursor;
        Vector2f dragVector;                //the amount of the drag the mouse cursor has
        Vector2f rDragVector;               //drag vector for rmb
        bool lmbDown;                       //is the left mouse button down?
        bool rmbDown;                       //the right mouse button down?
        static float minDrag = 50.0f;        //minimum amount of mouse movement to count as a drag
        EasyGraphNode hoveredNode;          //the node the cursor is hovering over. If there is none this is null
        EasyGraphNode startDragNode;

        public EasyModeState()
        {
            //mouse related initializations
            hoveredNode = null;
            startDragNode = null;
            oldCursor = new Vector2f(0.0f, 0.0f);
            dragVector = new Vector2f(0.0f, 0.0f);
            rDragVector = new Vector2f(0.0f, 0.0f);
            lmbDown = false;
            rmbDown = false;

            font = new Font("sansation.ttf");

            instructionText = new Text("Instructions:\nLeft click on empty space to make a new node.\nDrag the left mouse button from one node to another to make an edge.\nRight click on a node to delete it.\nPress up or down to change the weight of your next edge\nThe shortest path from the enemy node to the player node will be automatically calculated", font);
            instructionText.Position = new Vector2f(10.0f, 400.0f);
            instructionText.CharacterSize = 20;

            selectedWeight = 0;
            weightText = new Text("Current Weight: " + selectedWeight.ToString(), font);
            weightText.Position = new Vector2f(10.0f, 380.0f);
            weightText.CharacterSize = 20;

            

            graph = new EasyGraph();
            CreateLevelGrid(1);


            playerIndicator = new Text("Player", font);
            enemyIndicator = new Text("Enemy", font);
            playerIndicator.CharacterSize = 12;
            enemyIndicator.CharacterSize = 12;
            playerIndicator.Position = playerNode.getPosition() + new Vector2f(-playerNode.getRadius(), playerNode.getRadius() + 2.0f);
            enemyIndicator.Position = enemyNode.getPosition() + new Vector2f(-enemyNode.getRadius(), enemyNode.getRadius() + 2.0f);
            playerIndicator.Color = Color.Green;
            enemyIndicator.Color = Color.Red;
        }

        override public void Update(Time timeElapsed)
        {
            //don't really have much to do here, but if we want animations, this is probably where the logic for that would go
        }

        override public void Draw(RenderWindow window)
        {
            window.Draw(instructionText);
            window.Draw(weightText);
            window.Draw(enemyIndicator);
            window.Draw(playerIndicator);
            graph.Draw(window);
            

        }

        override public SystemMessage HandleKeyInput(Window window, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                return new PopStateMessage(this);
            }
            else if(e.Code == Keyboard.Key.Up)
            {
                selectedWeight++;
                weightText.DisplayedString = "Current Weight: " + selectedWeight.ToString();
            }
            else if(e.Code == Keyboard.Key.Down)
            {
                if(selectedWeight > 0)  //we can't deal with negative edge weights using Dijkstras
                {
                    selectedWeight--;
                    weightText.DisplayedString = "Current Weight: " + selectedWeight.ToString();
                }
            }
            return null;
        }

        override public SystemMessage HandleMouseMoveInput(Window window, MouseMoveEventArgs e)
        {
            //figure out if we're hovering over a node
            bool isHovering = false; 
            foreach(EasyGraphNode n in graph.getNodeList())
            {
                Vector2f nodePos = n.getPosition();
                Vector2f curPos = new Vector2f(e.X, e.Y);
                Vector2f delta = nodePos - curPos;
                if (delta.X * delta.X + delta.Y * delta.Y <= n.getRadius() * n.getRadius())
                {
                    hoveredNode = n;
                    isHovering = true;
                }
                
            }
            if (!isHovering)
                hoveredNode = null;

            //are we dragging the mouse?
            if(lmbDown)
            {
                dragVector.X += e.X - oldCursor.X;
                dragVector.Y += e.Y - oldCursor.Y;
            }
            else if(rmbDown)
            {
                rDragVector.X += e.X - oldCursor.X;
                rDragVector.Y += e.Y - oldCursor.Y;
            }
            oldCursor.X = e.X;
            oldCursor.Y = e.Y;
            return null;
        }
        override public SystemMessage HandleMousePressInput(Window window, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)      //has the LMB been pressed?
            {
                dragVector.X = 0;
                dragVector.Y = 0;
                lmbDown = true;

                if (hoveredNode != null)
                {
                    startDragNode = hoveredNode;
                }
            }
            else if(e.Button == Mouse.Button.Right)
            {
                rDragVector.X = 0;
                rDragVector.Y = 0;
                rmbDown = true;
            }
            return null;
        }
        override public SystemMessage HandleMouseReleaseInput(Window window, MouseButtonEventArgs e)
        {
            
            if (e.Button == Mouse.Button.Left)
            {
                //if the player just clicked and not dragged on a location without a node, make a node there
                if (hoveredNode == null & Utility.Instance.getVector2fLengthSquared(dragVector) <= minDrag)
                {
                    EasyGraphNode n = new EasyGraphNode(new Vector2f(e.X, e.Y));
                    graph.AddNode(n);
                }

                //if the player dragged the mouse from one node to another
                if (hoveredNode != null && startDragNode != null && hoveredNode != startDragNode)
                {
                    //make an edge between the two
                    graph.MakeEdge(startDragNode, hoveredNode, selectedWeight);
                    graph.FindPathFromEnemy(enemyNode, playerNode);
                }

                Console.WriteLine("Drag vector: " + dragVector);
                lmbDown = false;
                startDragNode = null;
            }
            else if(e.Button == Mouse.Button.Right)
            {
                //right-clicking a node
                if(hoveredNode != null && Utility.Instance.getVector2fLengthSquared(rDragVector) <= minDrag)
                {
                    if (hoveredNode != enemyNode && hoveredNode != playerNode)
                    {
                        graph.RemoveNode(hoveredNode);
                        graph.FindPathFromEnemy(enemyNode, playerNode);
                    }
                    else
                    {
                        Console.WriteLine("You cannot delete that node! It is either the player or the enemy");
                    }
                }
                rmbDown = false;
            }
            return null;
        }
        override public SystemMessage HandleMouseWheelInput(Window window, MouseWheelEventArgs e) { return null; }

        //A function for creating a 
        private void CreateLevelGrid(int level)
        {
            graph.Clear();

            Console.WriteLine("Building graph for level " + level + "...");
            if (level == 0)
            {
                EasyGraphNode center;
                EasyGraphNode north;
                EasyGraphNode south;
                EasyGraphNode east;
                EasyGraphNode west;
                EasyGraphNode wuuu;

                center = graph.MakeNode(new Vector2f(400.0f, 300.0f));
                north = graph.MakeNode(new Vector2f(400.0f, 200.0f));
                south = graph.MakeNode(new Vector2f(400.0f, 400.0f));
                east = graph.MakeNode(new Vector2f(500.0f, 300.0f));
                west = graph.MakeNode(new Vector2f(300.0f, 300.0f));
                wuuu = graph.MakeNode(new Vector2f(400.0f, 400.0f));

                graph.MakeEdge(center, north, 4);
                graph.MakeEdge(center, south, 4);
                graph.MakeEdge(center, west, 4);
                graph.MakeEdge(center, east, 4);
                graph.MakeEdge(north, east, 10);
                graph.MakeEdge(south, west, 20);
                graph.MakeEdge(west, north, 20);
                graph.MakeEdge(east, south, 50);
                graph.MakeEdge(north, wuuu, 900);

                enemyNode = east;
                playerNode = wuuu;
                graph.FindPathFromEnemy(east, wuuu);
            }
            else if (level == 1)
            {

                enemyNode = graph.MakeNode(new Vector2f(750.0f, 50.0f));
                playerNode = graph.MakeNode(new Vector2f(50.0f, 350.0f));

                graph.MakeEdge(enemyNode, playerNode, 400);
                graph.FindPathFromEnemy(enemyNode, playerNode);
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
