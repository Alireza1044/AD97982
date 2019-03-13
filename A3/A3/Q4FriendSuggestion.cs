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
            public Child(int destination, long weight)
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
            public Node(int key, long distance = Max, bool isChecked = false)
            {
                Key = key;
                Distance = distance;
                IsChecked = isChecked;
                Children = new List<Child>();
            }
        }


        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {

            List<long> result = new List<long>();

            Node[] graph = new Node[NodeCount + 1];
            Node[] graphR = new Node[NodeCount + 1];
            long[] dist = new long[graph.Length];
            long[] distR = new long[graph.Length];

            BuildGraph(edges, ref graph);
            BuildReverseGraph(edges, ref graphR);

            for (int i = 0; i < QueriesCount; i++)
            {
                for (int j = 1; j < graph.Length; j++)
                {
                    dist[j] = graph[j].Distance;
                    distR[j] = graphR[j].Distance;
                }              
                long temp = BiDirectionalDijkstra(graph, graphR, Queries[i][0], Queries[i][1], dist, distR);
                result.Add(temp);
            }
            return result.ToArray();
        }
        public static void BuildGraph(long[][] edges, ref Node[] graph)
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
        public static void BuildReverseGraph(long[][] edges, ref Node[] graph)
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

        public long BiDirectionalDijkstra(Node[] Graph, Node[] GraphR, long startNode, long endNode, long[] dist, long[] distR)
        {
            if (startNode == endNode)
                return 0;

            bool[] proc = new bool[Graph.Length];
            bool[] procR = new bool[Graph.Length];

            dist[(int)startNode] = 0;
            distR[(int)endNode] = 0;

            while (true)
            {

                int tempKey = FindMin(dist, proc);

                var temp = Graph[tempKey];

                for (int j = 0; j < temp.Children.Count(); j++)//process
                {
                    if (dist[tempKey] + temp.Children[j].Weight < dist[temp.Children[j].Destination])//relax
                    {
                        dist[temp.Children[j].Destination] = dist[tempKey] + temp.Children[j].Weight;
                    }
                }
                proc[tempKey] = true;
                if (procR[tempKey])
                {
                    return ShortestPath(startNode, Graph, proc, dist, endNode, GraphR, procR, distR);
                }

                //do the same for Reverse
                int tempRKey = FindMin(distR, procR);

                var tempR = GraphR[tempRKey];

                for (int j = 0; j < tempR.Children.Count(); j++)//process
                {
                    if (distR[tempRKey] + tempR.Children[j].Weight < distR[tempR.Children[j].Destination])//relax
                    {
                        distR[tempR.Children[j].Destination] = distR[tempRKey] + tempR.Children[j].Weight;
                    }
                }
                procR[tempRKey] = true;

                if (proc[tempR.Key])
                {
                    return ShortestPath(startNode, Graph, proc, dist, endNode, GraphR, procR, distR);
                }

            }
        }

        private int FindMin(long[] dist, bool[] proc)
        {
            long temp = long.MaxValue;
            int idx = -1;

            for (int i = 1; i < dist.Length; i++)
            {
                if (dist[i] < temp && !proc[i])
                {
                    temp = dist[i];
                    idx = i;
                }
            }
            return idx;
        }

        public long ShortestPath(long startNode, Node[] graph, bool[] proc
            , long[] dist, long endNode, Node[] graphR, bool[] procR, long[] distR)
        {
            long distance = Max;

            for (int i = 1; i < proc.Length; i++)
            {
                if (proc[i] || procR[i])
                {
                    if (dist[i] + distR[i] < distance)
                        distance = dist[i] + distR[i];
                }
            }

            if (distance == Max)
                return -1;
            else
                return distance;
        }
    }
}
