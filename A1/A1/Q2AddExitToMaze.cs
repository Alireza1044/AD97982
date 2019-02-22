using System;
using System.Collections.Generic;
using TestCommon;

namespace A12
{
    public class Q2AddExitToMaze : Processor
    {
        public Q2AddExitToMaze(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            int connectedCount = 0;
            Edge[] Graph = new Edge[nodeCount + 1];
            for (int i = 1; i < Graph.Length; i++)
            {
                Graph[i] = new Edge(i);
            }
            //build the graph
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                for (int j = 0; j < edges[i].Length; j++)
                {
                    if (!Graph[edges[i][0]].Connected.Contains(edges[i][1]))
                        Graph[edges[i][0]].Connected.Add(edges[i][1]);
                    if (!Graph[edges[i][1]].Connected.Contains(edges[i][0]))
                        Graph[edges[i][1]].Connected.Add(edges[i][0]);
                }
            }

            for (int i = 1; i < Graph.Length; i++)
            {
                if (!Graph[i].IsVisited)
                {
                    connectedCount++;
                    DFS(Graph, edges, i);
                }
            }
            return connectedCount;
        }

        public int DFS(Edge[] graph, long[][] edges, long startnode)
        {
            Queue<long> queue = new Queue<long>();
            queue.Enqueue(graph[startnode].Key);
            graph[startnode].IsVisited = true;
            while (queue.Count > 0)
            {
                long temp = queue.Dequeue();
                for (int i = 0; i < graph[temp].Connected.Count; i++)
                {
                    if (!graph[graph[temp].Connected[i]].IsVisited)
                    {
                        queue.Enqueue(graph[temp].Connected[i]);
                        graph[graph[temp].Connected[i]].IsVisited = true;
                    }
                }
            }
            return 0;
        }
    }
}
