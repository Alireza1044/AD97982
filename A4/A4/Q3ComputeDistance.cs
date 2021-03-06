﻿using System;
using System.Collections.Generic;
using System.IO;
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
            TestTools.Process(inStr, (Func<long, long, long[][], long[][], long, long[][], long[]>)Solve);


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

        public static double CalculatePotential(Node[] graph, Node currentNode, Node endNode, double[] dist)
        {
            var x2 = Math.Pow(endNode.CoOrds.X - currentNode.CoOrds.X, 2);
            var y2 = Math.Pow(endNode.CoOrds.Y - currentNode.CoOrds.Y, 2);
            return Math.Pow(x2 + y2, 0.5) + dist[currentNode.Key];

        }

        public static void BuildGraph(Node[] graph, long[][] points, long[][] edges)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i, points[i - 1][0], points[i - 1][1]);
            }
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph[edges[i][0]].Children.Add(new Edge((int)edges[i][1], graph[edges[i][1]].CoOrds, edges[i][2]));
            }
        }

        public static double CalculateDistance(Node[] graph, long startNode, long endNode, double[] dist, bool[] isProcessed)
        {
            if (startNode == endNode)
                return 0;

            bool[] open = new bool[graph.Length];
            bool[] closed = new bool[graph.Length];
            int openCount = 0;
            dist[startNode] = 0;
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i].Potential = CalculatePotential(graph, graph[i], graph[endNode], dist);
            }
            open[(int)startNode] = true;
            MinHeap heap = new MinHeap(graph.Length - 1);
            heap.BuildHeap(graph, (int)startNode);
            openCount++;
            Node temp = new Node();

            while (openCount > 0)
            {
                temp = graph[heap.ExtractMin()];

                if (temp.Key == endNode)
                    return dist[temp.Key];

                open[temp.Key] = false;
                openCount--;
                closed[temp.Key] = true;

                for (int i = 0; i < temp.Children.Count; i++)
                {
                    if (closed[temp.Children[i].Key])
                        continue;

                    var tentative_dist = dist[temp.Key] + temp.Children[i].Weight;

                    if (!open[temp.Children[i].Key])
                    {
                        open[temp.Children[i].Key] = true;
                        openCount++;
                    }
                    else if (tentative_dist >= dist[temp.Children[i].Key])
                        continue;

                    dist[temp.Children[i].Key] = tentative_dist;

                    if (dist[temp.Children[i].Key] +
                        CalculatePotential(graph, graph[temp.Children[i].Key], graph[endNode], dist)
                        < graph[temp.Children[i].Key].Potential)
                    {
                        graph[temp.Children[i].Key].Potential =
                            dist[temp.Children[i].Key] +
                            CalculatePotential(graph, graph[temp.Children[i].Key], graph[endNode], dist);

                        heap.ChangePriority(temp.Children[i].Key, graph[temp.Children[i].Key].Potential);
                    }
                }
            }
            return -1;
        }

        private static Node FindMinNode(Node[] graph, bool[] open, double[] dist)
        {
            double potential = Max;
            int idx = -1;
            for (int i = 1; i < open.Length; i++)
            {
                if (open[i])
                {
                    var key = graph[i].Key;
                    if (graph[key].Potential < potential)
                    {
                        idx = key;
                        potential = graph[key].Potential;
                    }
                }
            }
            return graph[idx];
        }
    }
}