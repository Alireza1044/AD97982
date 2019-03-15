using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using static A4.Q1BuildingRoads;

namespace A4
{
    public class Q3ComputeDistance : Processor
    {
        public Q3ComputeDistance(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long,long, long[][], long[][], long, long[][], long[]>)Solve);


        public long[] Solve(long nodeCount,
                            long edgeCount,
                            long[][] points,
                            long[][] edges,
                            long queriesCount,
                            long[][] queries)
        {
            Node[] graph = new Node[nodeCount + 1];
            BuildGraph(graph, points, edges);
            double[] dist = new double[nodeCount + 1];
            List<long> result = new List<long>();
            for (int i = 0; i < queries.GetLength(0); i++)
            {
                var startNode = queries[i][0];
                var endNode = queries[i][1];
                CalculatePotential(graph, graph[endNode]);
                dist[0] = Max + 1;
                for (int j = 1; j < dist.Length; j++)
                {
                    dist[j] = Max;
                }
                bool[] isProcessed = new bool[nodeCount + 1];
                result.Add((long)CalculateDistance(graph, startNode, endNode, dist, isProcessed));
            }
            return result.ToArray();
        }

        private void CalculatePotential(Node[] graph,Node endNode)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                var x2 = Math.Pow(endNode.CoOrds.X - graph[i].CoOrds.X, 2);
                var y2 = Math.Pow(endNode.CoOrds.Y - graph[i].CoOrds.Y, 2);
                graph[i].Potential = Math.Pow(x2 + y2, 0.5);
            }
        }

        public static void BuildGraph(Node[] graph, long[][] points,long[][] edges)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i, points[i-1][0], points[i-1][1]);
            }
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph[edges[i][0]].Children.Add(new Edge((int)edges[i][1], graph[edges[i][1]].CoOrds, edges[i][2]));
            }
        }

        public static double CalculateDistance(Node[] graph, long startNode,long endNode,double[] dist,bool[] isProcessed)
        {
            if (startNode == endNode)
                return 0;

            double result = 0;
            List<int> path = new List<int>();
            dist[startNode] = 0;
            path.Add((int)startNode);
            var temp = graph[startNode];
            while (true)
            {
                for (int i = 0; i < temp.Children.Count; i++)
                {
                    if(dist[temp.Key] + temp.Children[i].Weight < dist[temp.Children[i].Key] 
                        && !isProcessed[temp.Children[i].Key])
                        dist[temp.Children[i].Key] = dist[temp.Key] + temp.Children[i].Weight;
                }
                isProcessed[temp.Key] = true;
                temp = FindMinNode(graph,temp, dist, isProcessed);
                if (temp.Key == 0)
                    break;
                result = dist[temp.Key];
                if (temp.Key == endNode)
                    return result;
                path.Add(temp.Key);
            }

            return -1;
        }

        private static Node FindMinNode(Node[] graph, Node temp, double[] dist, bool[] isProcessed)
        {
            double distance = Max,potential = Max;
            int idx = 0;
            for (int i = 0; i < temp.Children.Count; i++)
            {
                var key = temp.Children[i].Key;
                if (graph[key].Potential + dist[key] < potential && !isProcessed[key])
                {
                    idx = key;
                    potential = graph[key].Potential + dist[key];
                    distance = dist[key];
                }
            }
            dist[idx] = distance;
            return graph[idx];
        }
    }
}