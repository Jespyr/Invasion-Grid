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
//Assignment 2

using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace InvasionGrid
{
    class EasyGraphNode
    {
        //Rendering stuff
        private CircleShape shape;

        //AI stuff (Dijkstra)
        public bool visited;            //whether this node has been visited by Dijkstra's algorithm (should this be public?)
        public EasyGraphNode parent;    //the parent node in a shortest path from the enemy
        public double distance;            //the distance to the parent

        private LinkedList<EasyGraphEdge> edges;

        public EasyGraphNode(Vector2f position)
        {
            edges = new LinkedList<EasyGraphEdge>();

            shape = new CircleShape(20.0f);
            shape.Origin = new Vector2f(20.0f, 20.0f);  //set the origin to the center of the object to make things easier
            shape.Position = position;
            shape.FillColor = Color.Transparent;
            shape.OutlineColor = Color.Cyan;
            shape.OutlineThickness = 2.0f;

            //Dijkstras
            visited = false;
            parent = null;
            distance = double.PositiveInfinity;
        }

        public void addEdge(EasyGraphNode target, int weight)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target", "Cannot add edge with null target!");
            }

            //artificial weight restrictions for easy mode. For now, just stick to weights >= 0
            if(weight < 0)
            {
                Console.WriteLine("Cannot make an edge with weights < 0 in easy mode");
                return;
            }

            
            foreach (EasyGraphEdge edge in edges)
            {
                if (edge.getTarget() == target)
                {
                    Console.WriteLine("Cannot add an edge here, one already exists!");
                    return;
                }
            }

            //finally add the edge
            edges.AddLast(new EasyGraphEdge(this, target, weight));
        }

        public void addEdge(EasyGraphEdge edge)
        {
            if(edge == null)
            {
                throw new ArgumentNullException("edge", "Exception! Cannot add null edge");
            }
            edges.AddLast(edge);
        }

        public LinkedList<EasyGraphEdge> getEdges()
        {
            return edges;
        }

        public void RemoveEdgeByTarget(EasyGraphNode target)
        {
            //need to make a stack/queue or some other structure, because we can't remove edges
            //immediately
            Stack<EasyGraphEdge> toRemove = new Stack<EasyGraphEdge>(); 
            foreach(EasyGraphEdge edge in edges)
            {
                if (edge.getTarget() == target)
                    toRemove.Push(edge);
            }
            
            while(toRemove.Count > 0)
            {
                edges.Remove(toRemove.Pop());
            }
        }

        public EasyGraphEdge GetEdgeByTarget(EasyGraphNode target)
        {
            foreach(EasyGraphEdge e in edges)
            {
                if(e.getTarget() == target)
                {
                    return e;
                }
            }
            return null;
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

            line.Size = new Vector2f(4.0f, lineLength - (source.getRadius() + target.getRadius()) - 4.0f);              //could just use one of the radii * 2, but this leaves the option of variable node sizes open
            line.Origin = line.Size / 2.0f;                                                                             //puts the origin at the center of the line
            line.Position = source.getPosition() + vec / 2.0f;
            line.Rotation = (float)(Math.Atan2(vec.Y, vec.X) * 180.0f / Math.PI)+ 90.0f;
            line.FillColor = Color.Red;            

            //the point of the arrow
            arrow = new CircleShape(10.0f, 3);                                                                          //a circle shape with 3 sides is a triangle
            arrow.Origin = new Vector2f(arrow.Radius, arrow.Radius);
            arrow.Rotation = line.Rotation;
            arrow.Position = target.getPosition() + -unitVec * (target.getRadius() + arrow.Radius + 2.0f);              //back off enough so the arrow points to the edge of the target
            arrow.FillColor = line.FillColor;


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

        public void Highlight()
        {
            line.FillColor = Color.Blue;
            arrow.FillColor = Color.Blue;
        }
        public void Unhighlight()
        {
            line.FillColor = Color.Red;
            arrow.FillColor = Color.Red;
        }
    }



    class EasyGraph
    {

        private List<EasyGraphNode> nodeList;
        private List<EasyGraphEdge> edgeList;

        public EasyGraph()
        {
            nodeList = new List<EasyGraphNode>();
            edgeList = new List<EasyGraphEdge>();
        }

        public void Clear()
        {
            Console.WriteLine("Clearing all edges and nodes");
            nodeList.Clear();
        }

        public void AddNode(EasyGraphNode n)
        {
            if (nodeList.Contains(n))
            {
                Console.WriteLine("This node already exists!");
                return;
            }
            nodeList.Add(n);
        }

        public EasyGraphNode MakeNode(Vector2f position)
        {
            EasyGraphNode newNode = new EasyGraphNode(position);
            nodeList.Add(newNode);
            return newNode;
        }

        public void RemoveNode(EasyGraphNode node)
        {
            //remove all edges pointing to the soon to be nonexistent node
            foreach (EasyGraphNode n in nodeList)
            {
                n.RemoveEdgeByTarget(node);

            }

            nodeList.Remove(node);

        }

        public void MakeEdge(EasyGraphNode sourceNode, EasyGraphNode destNode, int weight)
        {
            if (sourceNode == null)
            {
                throw new ArgumentNullException("sourceNode", "Exception: Cannot make edge, destNode is null");
            }
            else if (destNode == null)
            {
                throw new ArgumentNullException("destNode", "Exception: Cannot make edge, destNode is null");
            }

            EasyGraphEdge newEdge = new EasyGraphEdge(sourceNode, destNode, weight);
            edgeList.Add(newEdge);
            sourceNode.addEdge(newEdge);
        }


        public void RemoveEdge(EasyGraphNode sourceNode, EasyGraphNode destNode)
        {
            if (sourceNode == null)
            {
                throw new ArgumentNullException("sourceNode", "Exception: Cannot remove edge, destNode is null");
            }
            else if (destNode == null)
            {
                throw new ArgumentNullException("destNode", "Exception: Cannot remove edge, destNode is null");
            }

            sourceNode.RemoveEdgeByTarget(destNode);

        }

        public void FindPathFromEnemy(EasyGraphNode sourceNode, EasyGraphNode targetNode)
        {
            if(sourceNode == null || targetNode == null)
            {
                Console.Write("Cannot find path, either no source node, or no target node");
                return;
            }

            //unhighlight the current path
            foreach(EasyGraphEdge e in edgeList)
            {
                e.Unhighlight();
            }

            //set some starting values
            foreach (EasyGraphNode n in nodeList)
            {
                n.visited = false;
                n.parent = null;
                n.distance = double.PositiveInfinity;
            }

            NodePriorityQueue Q = new NodePriorityQueue(nodeList.Count);
            Q.Enqueue(sourceNode);
            sourceNode.visited = true;
            sourceNode.distance = 0;
            sourceNode.parent = null;
            while (!Q.isEmpty())
            {
                EasyGraphNode curNode = Q.Dequque();
                foreach (EasyGraphEdge edge in curNode.getEdges())
                {
                    EasyGraphNode next = edge.getTarget();
                    if (!next.visited)
                    {
                        next.visited = true;
                        next.distance = curNode.distance + edge.getWeight();
                        next.parent = curNode;
                        Q.Enqueue(next);
                    }
                    else
                    {
                        if (next.distance > curNode.distance + edge.getWeight())
                        {
                            next.distance = curNode.distance + edge.getWeight();
                            next.parent = curNode;
                            Q.Enqueue(next);
                        }
                    }
                }
            }

            //let's highlight the new path! (probably should put this in a different function)
            EasyGraphNode tmpNode = targetNode;
            while (tmpNode != null && tmpNode != sourceNode)
            {
                tmpNode.parent.GetEdgeByTarget(tmpNode).Highlight();
                tmpNode = tmpNode.parent;
            }

        }

        //code for drawing our graph on the screen using SFML
        public void Draw(RenderWindow window)
        {
            foreach (EasyGraphNode n in nodeList)
            {
                n.Draw(window);
            }
        }

        public List<EasyGraphNode> getNodeList()
        {
            return nodeList;
        }
    }

    //we have to make our own priority queue class apparently because .Net doesn't have one :(
    class NodePriorityQueue
    {

        private EasyGraphNode[] data;
        private int effectiveLength;

        public NodePriorityQueue(int size)
        {
            //instead of dealing with weird .Net list restrictions concerning .Count, etc
            //make an array that can hold all the nodes (the queue will never need more than
            //that)
            data = new EasyGraphNode[size];
            effectiveLength = 0;
        }

        public void Enqueue(EasyGraphNode node)
        {
            if(data.Length == effectiveLength)
            {
                throw new OverflowException("Queue full! Cannot enqueue");
            }

            effectiveLength++;
            int i;
            for(i = effectiveLength-1; i > 0 && node.distance < data[Parent(i)].distance; i = Parent(i))
            {
                data[i] = data[Parent(i)];
            }
            data[i] = node;
        }
        public EasyGraphNode Dequque()
        {
            EasyGraphNode min = data[0];

            data[0] = data[effectiveLength-1];

            effectiveLength--;
            Heapify(0);
            return min;
        }
        public EasyGraphNode Peek()
        {
            return data[0];
        }

        public bool isEmpty()
        {
            return effectiveLength == 0;
        }


        public void BuildHeap()
        {
            effectiveLength = data.Length - 1;
            for(int i = effectiveLength / 2; i >= 0; i--)
            {
                Heapify(i);
            }
        }

        public void Heapify(int i)
        {
            int smallestIndex;
            int l = LeftChild(i);
            int r = RightChild(i);

            if (l < effectiveLength && data[l].distance < data[i].distance)
            {
                smallestIndex = l;
            }
            else
            {
                smallestIndex = i;
            }

            if (r < effectiveLength && data[r].distance < data[smallestIndex].distance)
            {
                smallestIndex = r;
            }

            if (i != smallestIndex)
            {
                Swap(i, smallestIndex);
                Heapify(smallestIndex);
            }
        }

        private int Parent(int i)
        {
            return (i - 1) / 2;
        }
        private int LeftChild(int i)
        {
            return 2 * i + 1;
        }
        private int RightChild(int i)
        {
            return 2 * i + 2;
        }
        private void Swap(int first, int second)        //swap two items in the priority queue
        {
            if (first < 0 || first >= data.Length)
            {
                throw new IndexOutOfRangeException("NodePriorityQueue.Swap() failed, first index out of bounds");
            }
            if (second < 0 || second >= data.Length)    //should i use effective length instead?
            {
                throw new IndexOutOfRangeException("NodePriorityQueue.Swap() failed, second index out of bounds");
            }

            EasyGraphNode tmp = data[first];
            data[first] = data[second];
            data[second] = tmp;
        }
    }

        
    
}
