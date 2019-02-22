using System;
using System.Collections.Generic;
using TestCommon;

namespace A12
{
    public class Q3Acyclic : Processor
    {
        public Q3Acyclic(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
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
                    {
                        Graph[edges[i][0]].Connected.Add(edges[i][1]);
                        Graph[edges[i][1]].InDegree++;
                    }

                    if (!Graph[edges[i][1]].Parents.Contains(edges[i][0]))
                    {
                        Graph[edges[i][1]].Parents.Add(edges[i][0]);
                    }
                }
            }
            //for (int i = 1; i < Graph.Length; i++)
            //{
            //    if (!Graph[i].IsVisited)
            //    {
            //        flag = DFS(Graph, edges, i);
            //        if (flag)
            //            return 1;
            //    }
            //}

            return BFS(Graph, edges,nodeCount) ? 1:0;
        }

        public bool BFS(Edge[] graph, long[][] edges,long nodeCount)
        {
            long visitedCount = 0;
            Queue<long> queue = new Queue<long>();
            for (int i = 1; i < graph.Length; i++)
            {
                if (graph[i].InDegree == 0)
                    queue.Enqueue(graph[i].Key);
            }

            while (queue.Count > 0)
            {
                long temp = queue.Dequeue();
                visitedCount++;
                for (int i = 0; i < graph[temp].Connected.Count; i++)
                {
                    graph[graph[temp].Connected[i]].InDegree--;
                    if (graph[graph[temp].Connected[i]].InDegree == 0)
                        queue.Enqueue(graph[temp].Connected[i]);
                }
            }

            if (visitedCount != nodeCount)
                return true;
            else
                return false;
        }
    }
}