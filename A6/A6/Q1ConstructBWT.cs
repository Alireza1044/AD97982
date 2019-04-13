using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A6
{
    public class Q1ConstructBWT : Processor
    {
        public Q1ConstructBWT(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String>)Solve);

        public string Solve(string text)
        {
            // write your code here
            List<string> res = new List<string>();
            var pattern = text.ToCharArray();
            for (int i = pattern.Length-1; i >= 0; i--)
            {
                string suffix = text.Substring(i);
                string prefix = text.Substring(0, i);
                string newPattern = suffix + prefix;
                res.Add(newPattern);
            }
            res.Sort();
            string result = null;
            for (int i = 0; i < res.Count; i++)
            {
                result += res[i][res[i].Length - 1];
            }
            return result;
        }
    }
}
