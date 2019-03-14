using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A4
{
    public class Q3ComputeDistance : Processor
    {
        public Q3ComputeDistance(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long,long, long[][], long[][], long, long[][], long[]>)Solve);


        public long[] Solve(long nodeCount, 
                            long edgeCount,
                            long[][] points,
                            long[][] edges,
                            long queriesCount,
                            long[][] queries)
        {
            //Write Your Code Here
            return new long[] { };
        }
    }
}
