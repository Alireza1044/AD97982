using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q3ExchangingMoney:Processor
    {
        public Q3ExchangingMoney(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, string[]>)Solve);

        long Max = 2_000_000_000;

        public string[] Solve(long nodeCount, long[][] edges,long startNode)
        {
            Node[] graph = new Node[nodeCount + 1];
            Q1MinCost.BuildGraph(edges, graph);
            var infinitePossible = BellmanFord(graph,edges ,startNode);
            List<int> NegativeCycle = new List<int>();
            while (infinitePossible.Any())
            {
                var temp = infinitePossible.Dequeue();
                TravelBack(graph,temp);
            }
            List<string> result = new List<string>();
            for (int i = 1; i < graph.Length; i++)
            {
                if (graph[i].Weight == Max)
                    result.Add("*");
                else if (graph[i].IsChecked)
                    result.Add("-");
                else result.Add(graph[i].Weight.ToString());
            }
            
            return result.ToArray();
        }

        private Node[] TravelBack(Node[] graph, Node CanNeg)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(CanNeg);
            List<int> negativeCycles = new List<int>();
            while (true)
            {
                var temp = queue.Dequeue();
                if (negativeCycles.Contains(temp.Key))
                {
                    graph[temp.Key].IsChecked = true;
                    foreach (var key in negativeCycles)
                    {
                        graph[key].IsChecked = true;
                        BFS(graph, graph[key]);
                        //foreach (var i in graph[key].Children)
                        //    i.Item1.IsChecked = true;
                    }
                    break;
                }
                else
                    negativeCycles.Add(temp.Key);
                if (temp.Prev != null)
                    queue.Enqueue(temp.Prev);
                else break;
            }
            return graph;
        }
        
        public Node[] BFS(Node[] graph,Node startNode)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(startNode);
            while (queue.Any())
            {
                Node temp = queue.Dequeue();
                temp.IsChecked = true;
                foreach (var child in temp.Children)
                {
                    if(!child.Item1.IsChecked)
                        queue.Enqueue(child.Item1);
                }
            }
            return graph;
        }
         
        private Queue<Node> BellmanFord(Node[] graph, long[][] edges, long startNode)
        {
            graph[startNode].Weight = 0;
            for (int k = 1; k < graph.Length - 1; k++)
            {
                for (int i = 1; i < graph.Length; i++)
                {
                    for (int j = 0; j < graph[i].Children.Count; j++)
                    {
                        if (graph[i].Children[j].Item1.Weight > graph[i].Weight + graph[i].Children[j].Item2
                            && graph[i].Weight != Max)
                        {
                            graph[i].Children[j].Item1.Weight = graph[i].Weight + graph[i].Children[j].Item2;
                            graph[i].Children[j].Item1.Prev = graph[i];
                        }
                    }
                }
            }

            Queue<Node> infinitePossible = new Queue<Node>();

            for (int i = 1; i < graph.Length; i++)
            {
                for (int j = 0; j < graph[i].Children.Count; j++)
                {
                    if (graph[i].Children[j].Item1.Weight > graph[i].Weight + graph[i].Children[j].Item2)
                    {
                        infinitePossible.Enqueue(graph[i].Children[j].Item1);
                    }
                }
            }
            return infinitePossible;
        }
    }
}
