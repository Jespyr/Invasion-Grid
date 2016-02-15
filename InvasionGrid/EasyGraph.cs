using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



//EasyGraph.cs
//This file contains graph & graph related objects
//pertaining to easy mode
//Notes:
//1. This implementation will be using adjency list representation,
//as a dense graph would make it unbearably difficult or impossible
//to play this game
//2. We need to be able to add, remove, and modify edges and vertices at will
//3. Nodes have to have data associated with them that allows them to be rendered
//by the primary system

//-Nick Carpenetti
//CSE 265 Winter 2016

using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace InvasionGrid
{
    class EasyGraphNode
    {
        //Rendering stuff
        private CircleShape shape;

        //AI stuff
        //note: should this be an edge object instead?
        private EasyGraphNode nextNode;     //the next node one needs to move to in order to reach the player with minimal damage

        private LinkedList<EasyGraphEdge> edges;

        public EasyGraphNode(Vector2f position)
        {
            edges = new LinkedList<EasyGraphEdge>();

            shape = new CircleShape(20.0f);
            shape.Origin = new Vector2f(20.0f, 20.0f);  //set the origin to the center of the object to make things easier
            shape.Position = position;
            shape.FillColor = Color.Green;
            //shape.OutlineColor = Color.Cyan;
        }

        public void addEdge(EasyGraphNode target, int weight)
        {
            if (target == null)
            {
                //exception?
                return;
            }

            //check if the edge already exists (necessary?)
            /*foreach (EasyGraphEdge edge in edges)
            {
                if (edge.getTarget() == target)
                {
                    //exception: edge already exists!
                }
            }*/


            //perhaps insert some artificial wight restrictions?

            //finally add the edge
            edges.AddLast(new EasyGraphEdge(this, target, weight));
        }

        public void removeEdgeByDestination(EasyGraphNode destination)
        {
            foreach(EasyGraphEdge edge in edges)            //i've got a nagging feeling this isn't as efficient as it could be
            {
                if (edge.getTarget() == destination)
                    edges.Remove(edge);
            }
        }


        public Vector2f getPosition()
        {
            return  shape.Position;
        }
        public float getRadius()
        {
            return shape.Radius;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(shape);

            foreach (EasyGraphEdge e in edges)
            {
                e.Draw(window);
            }

            
        }


    }
    
    class EasyGraphEdge
    {
        private EasyGraphNode source;
        private EasyGraphNode target;
        private int weight;

        //SFML
        private RectangleShape line;
        private CircleShape arrow;
        private Font weightFont;
        private Text weightText;

        public EasyGraphEdge(EasyGraphNode source, EasyGraphNode target, int weight)
        {
            this.source = source;
            this.target = target;
            this.weight = weight;

            line = new RectangleShape();

            Console.WriteLine("Making Edge:");
            Console.WriteLine("Source: " + source.getPosition());
            Console.WriteLine("Destination: " + target.getPosition());

            //some useful variables to clean up the folloing mess
            Vector2f vec = new Vector2f(target.getPosition().X - source.getPosition().X, target.getPosition().Y - source.getPosition().Y);
            float lineLength = (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
            Vector2f unitVec = vec / lineLength;

            line.Size = new Vector2f(4.0f, lineLength - (source.getRadius() + target.getRadius())); //could just use one of the radii * 2, but this leaves the option of variable node sizes open
            line.Origin = line.Size / 2.0f; //puts the origin at the center of the line
            line.Position = source.getPosition() + vec / 2.0f;
            line.Rotation = (float)(Math.Atan2(vec.Y, vec.X) * 180.0f / Math.PI)+ 90.0f;
            line.FillColor = Color.Red;

            Console.WriteLine("Vec: " + vec);
            Console.WriteLine("unitvec:" + unitVec);
            Console.WriteLine("line length: " + lineLength);
            Console.WriteLine("rotation: " + line.Rotation);

            

            //the point of the arrow
            arrow = new CircleShape(10.0f, 3);                   //a circle shape with 3 sides is a triangle
            arrow.Origin = new Vector2f(arrow.Radius, arrow.Radius);
            arrow.Rotation = line.Rotation;
            arrow.Position = target.getPosition() + -unitVec * (target.getRadius() + arrow.Radius - 2.0f); //back off enough so the arrow points to the edge of the target
            arrow.FillColor = line.FillColor;
            Console.WriteLine("arrowpos:" + arrow.Position);


            //the weight indicator
            weightFont = new Font("sansation.ttf");
            weightText = new Text(weight.ToString(), weightFont);
            weightText.CharacterSize = 20;
            weightText.Color = Color.Yellow;
            weightText.Style = Text.Styles.Bold;

            FloatRect textRect = weightText.GetLocalBounds();
            weightText.Origin = new Vector2f(textRect.Left + textRect.Width / 2, textRect.Top + textRect.Height / 2);
            weightText.Position = line.Position;
            
            
        }

        public EasyGraphNode getSource()
        {
            return source;
        }
        public EasyGraphNode getTarget()
        {
            return target;
        }

        public int getWeight()
        {
            return weight;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(line);
            window.Draw(arrow);
            window.Draw(weightText);
        }
    }

    class EasyGraph
    {
        
        private List<EasyGraphNode> NodeList;

        public EasyGraph()
        {
            NodeList = new List<EasyGraphNode>();
        }

        public void Clear()
        {
            NodeList.Clear();
        }

        public void AddNode(EasyGraphNode n)
        {
            if(NodeList.Contains(n))
            {
                //exception!
                return;
            }
            NodeList.Add(n);
        }

        public EasyGraphNode MakeNode(Vector2f position)
        {
            EasyGraphNode newNode = new EasyGraphNode(position);
            NodeList.Add(newNode);
            return newNode;
        }

        public void RemoveNode(EasyGraphNode node)
        {
            NodeList.Remove(node);
            //afterwards we need to remove all edges pointing
            //to the now nonexistent node
            foreach(EasyGraphNode n in NodeList)
            {
                //test if it has an edge pointing to n
                //remove the edge
                n.removeEdgeByDestination(node);
            }
        }
        
        public void MakeEdge(EasyGraphNode curNode, EasyGraphNode dest, int weight)
        {
               
            curNode.addEdge(dest, weight);
        }

        public void RemoveEdge(EasyGraphNode sourceNode, EasyGraphNode destNode)
        {

        }

        //code for drawing our graph on the screen using SFML
        public void Draw(RenderWindow window)
        {
            foreach(EasyGraphNode n in NodeList)
            {
                //draw the node as a 10 unit white circle (for now)
                n.Draw(window);
                
            }
        }

        
    }
}
