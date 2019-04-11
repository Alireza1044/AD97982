using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q3GeneralizedMPM : Processor
    {
        public Q3GeneralizedMPM(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        public long[] Solve(string text, long n, string[] patterns)
        {
            var x = new Q2MultiplePatternMatching("TD3");
            return x.Solve(text, n, patterns);
        }
    }
}
