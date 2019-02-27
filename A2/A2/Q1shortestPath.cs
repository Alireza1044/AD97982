using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A2
{
    public class Node
    {
        public List<Node> Connected { get; set; }
        public int Depth { get; set; }
        public int ID { get; set; }
        public bool IsChecked { get; set; }
        public string Color { get; set; }
        public Node(int id)
        {
            Connected = new List<Node>();
            Depth = -1;
            ID = id;
            Color = "None";
            IsChecked = false;
        }
    }
    public class Q1ShortestPath : Processor
    {
        public Q1ShortestPath(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);


        public long Solve(long NodeCount, long[][] edges, long StartNode, long EndNode)
        {
            //Write your code here
            Node[] Graph = new Node[NodeCount + 1];
            BuildTree(Graph, edges);
            return FindShortestPath(Graph, StartNode, EndNode);
        }

        private long FindShortestPath(Node[] graph, long startNode, long endNode)
        {
            graph[startNode].Depth = 0;
            Queue<Node> nodes = new Queue<Node>();
            nodes.Enqueue(graph[startNode]);

            while (nodes.Count != 0)
            {

                Node temp = nodes.Dequeue();
                temp.IsChecked = true;
                if (temp.ID == endNode)
                    return temp.Depth;
                foreach (var node in temp.Connected)
                {
                    if(node.Depth == -1)
                        node.Depth = temp.Depth + 1;
                    else if (node.Depth > temp.Depth + 1)
                        node.Depth = temp.Depth + 1;
                    if(node.IsChecked == false)
                        nodes.Enqueue(node);
                }
            }
            return -1;
        }
        public Node[] BuildTree(Node[] graph, long[][] edges)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i);
            }

            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph[edges[i][0]].Connected.Add(graph[edges[i][1]]);
                graph[edges[i][1]].Connected.Add(graph[edges[i][0]]);
            }
            return graph;
        }
    }
}
