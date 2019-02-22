using System;
using System.Collections.Generic;
using TestCommon;

namespace A12
{
    public class Edge
    {
        public List<long> Connected { get; set; }
        public List<long> Parents { get; set; }
        public List<long> ConnectedReverse { get; set; }
        public List<long> ParentsReverse { get; set; }
        public long Key { get; set; }
        public long InDegree { get; set; }
        public bool IsVisited { get; set; }
        public Edge(long key)
        {
            Key = key;
            IsVisited = false;
            InDegree = 0;
            Connected = new List<long>();
            Parents = new List<long>();
            ConnectedReverse = new List<long>(); ;
            ParentsReverse = new List<long>();
        }
    }

    public class Q1MazeExit : Processor
    {
        public Q1MazeExit(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);

        public long Solve(long nodeCount, long[][] edges, long StartNode, long EndNode)
        {
            Edge[] Graph = new Edge[nodeCount+1];
            for (int i = 1; i < Graph.Length; i++)
            {
                Graph[i] = new Edge(i);
            }
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
            return DFS(Graph,edges,StartNode,EndNode);
        }
        
        public int DFS(Edge[] graph,long[][]edges,long startnode,long endNode)
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
                if (queue.Contains(endNode))
                    return 1;
            }
            return 0;
        }   
     }
}
