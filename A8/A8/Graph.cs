using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A8
{
    public struct Edge
    {
        public int Key { get; set; }
        public long Capacity { get; set; }
        public long Flow { get; set; }
        public Edge(int key,long capacity)
        {
            Key = key;
            Capacity = capacity;
            Flow = 0;
        }
    }
    public class Graph
    {
        public int Key { get; set; }
        public List<Edge> Children { get; set; }
        public Graph(int key)
        {
            Key = key;
            Children = new List<Edge>();
        }
        internal static Graph[] BuildGraph(long nodeCount, long edgeCount, long[][] edges)
        {
            Graph[] graph = new Graph[nodeCount+1];
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Graph(i);
            }

            for (int i = 1; i < edges.Length; i++)
            {
                graph[edges[i][0]].Children.Add(new Edge((int)edges[i][1], edges[i][2]));
            }

            return graph;
        }

        internal static long FindMaxFlow(Graph[] graph, Graph[] rGraph,int source, int sink)
        {
            int[] parent = new int[graph.Length];
            long maxFlow = 0;

            while (BFS(rGraph, parent, source,sink))
            {
                long pathFlow = int.MaxValue;

                for (int i = sink; i != source; i = parent[i])
                {
                    if (i < 1)
                        break;
                    pathFlow = Math.Min(pathFlow, 
                        rGraph[parent[i]].Children.Find(x => x.Key == i).Capacity);
                }

                for (int i = sink; i != source; i = parent[i])
                {
                    if (i < 1)
                        break;
                    var temp = rGraph[parent[i]].Children.FindIndex(x => x.Key == i);
                    rGraph[parent[i]].Children[temp] = new Edge(rGraph[parent[i]].Children[temp].Key
                        , rGraph[parent[i]].Children[temp].Capacity - pathFlow);
                    temp = rGraph[i].Children.FindIndex(x => x.Key == parent[i]);
                    rGraph[i].Children[temp] = new Edge(rGraph[i].Children[temp].Key
                        , rGraph[i].Children[temp].Capacity + pathFlow);
                }

                maxFlow += pathFlow;
            }

            return maxFlow;
        }

        private static bool BFS(Graph[] rGraph, int[] parent, int source, int sink)
        {
            bool[] isChecked = new bool[rGraph.Length];

            // Create a queue, enqueue source vertex and mark 
            // source vertex as visited 
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);
            parent[source] = -1;
            isChecked[source] = true;
            // Standard BFS Loop 
            while (queue.Count != 0)
            {
                int temp = queue.Dequeue();

                for (int i = 0; i < rGraph[temp].Children.Count; i++)
                {
                    if (isChecked[rGraph[temp].Children[i].Key] == false 
                        && rGraph[temp].Children[i].Capacity > 0)
                    {
                        queue.Enqueue(rGraph[temp].Children[i].Key);
                        parent[rGraph[temp].Children[i].Key] = temp;
                        isChecked[rGraph[temp].Children[i].Key] = true;
                    }
                }
            }

            // If we reached sink in BFS  
            // starting from source, then 
            // return true, else false 
            return (isChecked[sink] == true);
        }
    }
}
