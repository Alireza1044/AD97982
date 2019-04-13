using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A6
{
    public class Q4ConstructSuffixArray : Processor
    {
        public Q4ConstructSuffixArray(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) => 
            TestTools.Process(inStr, (Func<String, long[]>)Solve);

        public long[] Solve(string text)
        {
            // write your code here
            List<long> result = new List<long>();
            List<(string, int)> suffixArray = new List<(string, int)>();

            for (int i = 0; i < text.Length; i++)
            {
                suffixArray.Add((text.Substring(i), i));
            }
            suffixArray = suffixArray.OrderBy(x => x.Item1).ToList();
            for (int i = 0; i < suffixArray.Count; i++)
            {
                result.Add(suffixArray[i].Item2);
            }
            return result.ToArray();
        }
    }
}
