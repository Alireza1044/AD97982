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
            //this.ExcludeTestCaseRangeInclusive(2, 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long[]>)Solve);


        public long[] Solve(long NodeCount, long[][] edges)
        {
            return new long[] { };
        }
    }
}
