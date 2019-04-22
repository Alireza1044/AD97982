using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam1
{
    public class Node
    {
        public List<int> Children { get; set; }
        public int Key { get; set; }
        public int Betweennes { get; set; }
        public Node(int key)
        {
            Children = new List<int>();
            Key = key;
            Betweennes = 0;
        }

        internal static Node[] BuildGraph(long nodeCount, long[][] edges)
        {
            Node[] graph = new Node[nodeCount + 1];
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Node(i);
            }
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                graph[edges[i][0]].Children.Add((int)edges[i][1]);
            }
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i].Children = graph[i].Children.OrderByDescending(x => x).ToList();
            }
            return graph;
        }

        internal static void FindShortestPaths(Node[] graph)
        {
            for (int i = 1; i < graph.Length; i++)
            {
                for (int j = 1; j < graph.Length; j++)
                {
                    bool[] isChecked = new bool[graph.Length];
                    int[] preNode = new int[graph.Length];
                    if (i == j || graph[i].Children.Contains(j))
                        continue;
                    Queue<int> queue = new Queue<int>();
                    int source = i;
                    int destination = j;
                    queue.Enqueue(source);
                    while (queue.Count != 0)
                    {
                        int temp = queue.Dequeue();

                        if (temp == destination)
                            break;

                        isChecked[temp] = true;

                        for (int k = 0; k < graph[temp].Children.Count; k++)
                        {
                            if (!isChecked[graph[temp].Children[k]] && preNode[graph[temp].Children[k]] == 0)
                            {
                                queue.Enqueue(graph[temp].Children[k]);
                                preNode[graph[temp].Children[k]] = temp;
                            }
                        }
                    }
                    int pivot = destination;
                    if (preNode[destination] == 0)
                        continue;
                    while (pivot != source)
                    {
                        graph[preNode[pivot]].Betweennes++;
                        pivot = preNode[pivot];
                    }
                    graph[source].Betweennes--;
                }
            }
        }
    }
}
