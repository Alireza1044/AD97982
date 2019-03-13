using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q4FriendSuggestion : Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long, long[][], long[]>)Solve);
        public const long Max = 2_000_000_000;

        public struct Child
        {
            public long Weight { get; set; }
            public int Destination { get; set; }
            public Child(int destination,long weight)
            {
                Weight = weight;
                Destination = destination;
            }
        }

        public struct Node
        {
            public long Distance { get; set; }
            public bool IsChecked { get; set; }
            public int Key { get; set; }
            public List<Child> Children { get; set; }
            //public List<Child> Parents { get; set; }
            public Node(int key,long distance = Max,bool isChecked = false)
            {
                Key = key;
                Distance = distance;
                IsChecked = isChecked;
                Children = new List<Child>();
                //Parents = new List<Child>();
            }
        }
        

        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {

            //Node[] graph = new Node[NodeCount + 1];
            //Node[] graphR = new Node[NodeCount + 1];
            List<long> result = new List<long>();

            Node[] graph = new Node[NodeCount+1];
            Node[] graphR = new Node[NodeCount + 1];

            BuildGraph(edges,ref graph);
            BuildReverseGraph(edges,ref graphR);

            for (int i = 0; i < QueriesCount; i++)
            {
                //for (int j = 1; j < graph.Count(); j++)
                //{
                //    graph[j].IsChecked = false;
                //    graphR[j].IsChecked = false;
                //    graph[j].Weight = Max;
                //    graphR[j].Weight = Max;
                //}

                //List<Node> graphList = graph.ToList();
                //List<Node> graphListR = graphR.ToList();              

                result.Add(BiDirectionalDijkstra(graph,graphR,Queries[i][0], Queries[i][1]));
            }
            return result.ToArray();
        }
        public static void BuildGraph(long[][] edges,ref Node[] graph)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i);
            }

            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph[edges[i][0]].Children.Add(new Child((int)edges[i][1], edges[i][2]));
            }
        }
        public static void BuildReverseGraph(long[][] edges,ref Node[] graph)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i);
            }
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph[edges[i][1]].Children.Add(new Child((int)edges[i][0], edges[i][2]));
            }
        }

        public long BiDirectionalDijkstra(Node[] Graph, Node[] GraphR, long startNode, long endNode)
        {
            if (startNode == endNode)
                return 0;

            bool[] proc = new bool[Graph.Length];
            bool[] procR = new bool[Graph.Length];

            Graph[(int)startNode].Distance = 0;
            GraphR[(int)endNode].Distance = 0;

            while (true)
            {

                int tempKey = FindMin(Graph,proc);

                var temp = Graph[tempKey];

                for (int j = 0; j < temp.Children.Count(); j++)//process
                {
                    if (temp.Distance + temp.Children[j].Weight < Graph[temp.Children[j].Destination].Distance)//relax
                    {
                        Graph[temp.Children[j].Destination].Distance = temp.Distance + temp.Children[j].Weight;
                    }
                }
                proc[tempKey] = true;
                if (procR[tempKey])
                {
                    return ShortestPath(startNode, Graph, proc, endNode, GraphR, procR);
                }

                //do the same for Reverse
                int tempRKey = FindMin(GraphR, procR);

                var tempR = GraphR[tempRKey];

                for (int j = 0; j < tempR.Children.Count(); j++)//process
                {
                    if (tempR.Distance + tempR.Children[j].Weight < Graph[tempR.Children[j].Destination].Distance)//relax
                    {
                        Graph[tempR.Children[j].Destination].Distance = tempR.Distance + tempR.Children[j].Weight;
                    }
                }
                procR[tempRKey] = true;

                if (proc[tempR.Key])
                {
                    return ShortestPath(startNode, Graph, proc, endNode, GraphR, procR);
                }

            }
        }

        private int FindMin(Node[] graph, bool[] proc)
        {
            long temp = long.MaxValue;
            int idx = -1;

            for (int i = 1; i < graph.Length; i++)
            {
                if (graph[i].Distance < temp && !proc[i])
                {
                    temp = graph[i].Distance;
                    idx = i;
                }
            }
            return idx;
        }

        public long ShortestPath(long startNode, Node[] graph, bool[] proc
            , long endNode, Node[] graphR, bool[] procR)
        {
            long distance = Max;

            for (int i = 1; i < proc.Length; i++)
            {
                if(proc[i] || procR[i])
                {
                    if (graph[i].Distance + graphR[i].Distance < distance)
                        distance = graph[i].Distance + graphR[i].Distance;
                }
            }

            if (distance == Max)
                return -1;
            else
                return distance;
        }
    }
}
