using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace A12
{
    public class Q5StronglyConnected: Processor
    {
        public Q5StronglyConnected(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        Stack<long> S;
        Stack<long> reverseOder;

        public long Solve(long nodeCount, long[][] edges)
        {
            S = new Stack<long>();
            reverseOder = new Stack<long>();
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
                        Graph[edges[i][1]].ConnectedReverse.Add(edges[i][0]);
                        Graph[edges[i][1]].InDegree++;
                    }

                    if (!Graph[edges[i][1]].Parents.Contains(edges[i][0]))
                    {
                        Graph[edges[i][1]].Parents.Add(edges[i][0]);
                        Graph[edges[i][0]].ParentsReverse.Add(edges[i][1]);
                    }
                }
            }

            for (int i = 1; i < Graph.Length; i++)
            {
                if (!Graph[i].IsVisited)
                {
                    DFS(Graph, edges, i);
                }
            }
            int result = 0;
            while (S.Count > 0)
            {
                long temp = S.Pop();
                if (Graph[Graph[temp].Key].IsVisited)
                {
                    ReverseDFS(Graph, edges, temp);
                    result++;
                }
            }
            return result;
        }

        public void DFS(Edge[] graph, long[][] edges, long startnode)
        {
            Stack<long> stack = new Stack<long>();
            stack.Push(startnode);
            while(stack.Count > 0)
            {
                long temp = stack.Pop();
                graph[temp].IsVisited = true;
                for (int i = 0; i < graph[temp].Connected.Count; i++)
                {
                    if (!graph[graph[temp].Connected[i]].IsVisited)
                    {
                        DFS(graph, edges, graph[temp].Connected[i]);
                    }
                }
                S.Push(temp);
            }
        }

        public void ReverseDFS(Edge[] graph, long[][] edges, long startnode)
        {
            Stack<long> stack = new Stack<long>();
            stack.Push(startnode);
            while (stack.Count > 0)
            {
                long temp = stack.Pop();
                graph[temp].IsVisited = false;
                for (int i = 0; i < graph[temp].ConnectedReverse.Count; i++)
                {
                    if (graph[graph[temp].ConnectedReverse[i]].IsVisited)
                    {
                        stack.Push(graph[temp].ConnectedReverse[i]);
                    }
                }
            }
        }
    }
}
