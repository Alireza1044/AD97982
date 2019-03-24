using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q2MultiplePatternMatching : Processor
    {
        public Q2MultiplePatternMatching(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        public long[] Solve(string text, long n, string[] patterns)
        {
            // write your code here
			throw new NotImplementedException();
        }
    }
}
