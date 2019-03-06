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
            //Write Your Code Here
            return 0;
        }
    }
}
