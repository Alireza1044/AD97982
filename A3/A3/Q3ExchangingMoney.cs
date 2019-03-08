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


        public string[] Solve(long nodeCount, long[][] edges,long startNode)
        {
            Node[] graph = new Node[nodeCount + 1];
            Q1MinCost.BuildGraph(edges, graph);
            var negKeys = BellmanFord(graph,edges ,startNode);
            List<string> result = new List<string>();
            for (int i = 1; i < graph.Length; i++)
            {
                if (negKeys.Contains(graph[i].Key))
                    result.Add("-");
                else if (graph[i].Key == 2000000000)
                    result.Add("*");
                else result.Add(graph[i].Weight.ToString());
            }
            return result.ToArray();
        }

        private List<int> BellmanFord(Node[] graph, long[][] edges, long startNode)
        {
            for (int k = 1; k < graph.Length - 1; k++)
            {
                for (int i = 1; i < graph.Length; i++)
                {
                    for (int j = 0; j < graph[i].Children.Count; j++)
                    {
                        if (graph[i].Children[j].Item1.Weight > graph[i].Weight + graph[i].Children[j].Item2)
                            graph[i].Children[j].Item1.Weight = graph[i].Weight + graph[i].Children[j].Item2;
                    }
                }
            }

            
            List<int> negativeKeys = new List<int>();
            for (int i = 1; i < graph.Length; i++)
            {
                for (int j = 0; j < graph[i].Children.Count; j++)
                {
                    if (graph[i].Children[j].Item1.Weight > graph[i].Weight + graph[i].Children[j].Item2)
                        negativeKeys.Add(graph[i].Children[j].Item1.Key);
                }
            }
            return negativeKeys;
        }
    }
}
