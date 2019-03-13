using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q2DetectingAnomalies:Processor
    {
        public Q2DetectingAnomalies(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);


        public long Solve(long nodeCount, long[][] edges)
        {
            Node[] graph = new Node[nodeCount + 1];
            Q1MinCost.BuildGraph(edges, graph);
            graph[1].Weight = 0;
            return BellmanFord(graph,edges);
        }

        private long BellmanFord(Node[] graph, long[][] edges)
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
            for (int i = 1; i < graph.Length; i++)
            {
                for (int j = 0; j < graph[i].Children.Count; j++)
                {
                    if (graph[i].Children[j].Item1.Weight > graph[i].Weight + graph[i].Children[j].Item2)
                        return 1;
                }
            }
            return 0;
        }
    }
}
