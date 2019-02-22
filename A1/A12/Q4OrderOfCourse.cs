using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestCommon;

namespace A12
{
    public class Q4OrderOfCourse: Processor
    {
        public Q4OrderOfCourse(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long[]>)Solve);

        public long[] Solve(long nodeCount, long[][] edges)
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
            var stack = DFS(Graph, edges, nodeCount);
            List<long> result = new List<long>();
            while(stack.Count != 0)
            {
                result.Add(stack.Pop());
            }
            result.Reverse();
            return result.ToArray();
        }

        public Stack<long> DFS(Edge[] graph, long[][] edges, long nodeCount)
        {
            long visitedCount = 0;
            Stack<long> stack = new Stack<long>();
            Queue<long> queue = new Queue<long>();
            for (int i = graph.Length-1; i > 0; i--)
            {
                if (graph[i].InDegree == 0)
                    queue.Enqueue(graph[i].Key);
            }

            while (queue.Count > 0)
            {
                long temp = queue.Dequeue();
                stack.Push(temp);
                visitedCount++;
                for (int i = 0; i < graph[temp].Connected.Count; i++)
                {
                    graph[graph[temp].Connected[i]].InDegree--;
                    if (graph[graph[temp].Connected[i]].InDegree == 0)
                        queue.Enqueue(graph[temp].Connected[i]);
                }
            }

            return stack;
        }

        public override Action<string, string> Verifier { get; set; } = TopSortVerifier;

        /// <summary>
        /// کد شما با متد زیر راست آزمایی میشود
        /// این کد نباید تغییر کند
        /// داده آزمایشی فقط یک جواب درست است
        /// تنها جواب درست نیست
        /// </summary>
        public static void TopSortVerifier(string inFileName, string strResult)
        {
            long[] topOrder = strResult.Split(TestTools.IgnoreChars)
                .Select(x => long.Parse(x)).ToArray();

            long count;
            long[][] edges;
            TestTools.ParseGraph(File.ReadAllText(inFileName), out count, out edges);

            // Build an array for looking up the position of each node in topological order
            // for example if topological order is 2 3 4 1, topOrderPositions[2] = 0, 
            // because 2 is first in topological order.
            long[] topOrderPositions = new long[count];
            for (int i = 0; i < topOrder.Length; i++)
                topOrderPositions[topOrder[i] - 1] = i;
            // Top Order nodes is 1 based (not zero based).

            // Make sure all direct depedencies (edges) of the graph are met:
            //   For all directed edges u -> v, u appears before v in the list
            foreach (var edge in edges)
                if (topOrderPositions[edge[0] - 1] >= topOrderPositions[edge[1] - 1])
                    throw new InvalidDataException(
                        $"{Path.GetFileName(inFileName)}: " +
                        $"Edge dependency violoation: {edge[0]}->{edge[1]}");

        }
    }
}
