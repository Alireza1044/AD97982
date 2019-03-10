using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q4FriendSuggestion:Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long,long[][], long[]>)Solve);

        public long[] Solve(long NodeCount, long EdgeCount, 
                              long[][] edges, long QueriesCount, 
                              long[][]Queries)
        {
            Node[] graph = new Node[NodeCount + 1];
            Q1MinCost.BuildGraph(edges, graph);
            CalculateDistance(graph,Queries);
            return new long[] { };
        }

        private void CalculateDistance(Node[] graph, long[][] queries)
        {
            
        }
    }
}
