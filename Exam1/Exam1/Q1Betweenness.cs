using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace Exam1
{
    public class Q1Betweenness : Processor
    {
        public Q1Betweenness(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(15, 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long[]>)Solve);


        public long[] Solve(long NodeCount, long[][] edges)
        {
            //edges = edges.OrderByDescending(x => x).ToArray();
            List<long> result = new List<long>();
            Node[] graph = Node.BuildGraph(NodeCount, edges);
            Node.FindShortestPaths(graph);
            for (int i = 1; i < graph.Length; i++)
            {
                result.Add(graph[i].Betweennes);
            }
            return result.ToArray();
        }
    }
}
