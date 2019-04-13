using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A6
{
    public class Q2ReconstructStringFromBWT : Processor
    {
        public Q2ReconstructStringFromBWT(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String>)Solve);

        public string Solve(string bwt)
        {
            // write your code here
            StringBuilder result = new StringBuilder();
            List<(char, int)> lastColumn = new List<(char, int)>();
            for (int i = 0; i < bwt.Length; i++)
            {
                lastColumn.Add((bwt[i], i));
            }
            var firstColumn = lastColumn.OrderBy(x => x.Item1).ToList();
            for (int i = 0; i < firstColumn.Count; i++)
            {
                lastColumn[firstColumn[i].Item2] =
                    (lastColumn[firstColumn[i].Item2].Item1, i);
            }

            int idx = 0;
            var temp = firstColumn[idx];

            while(result.Length < bwt.Length)
            {
                result.Append(temp.Item1);
                idx = lastColumn[idx].Item2;
                temp = firstColumn[idx];
            }
            var res = result.ToString().ToCharArray();
            res = res.Reverse().ToArray();
            //result.Append('$');
            
            return string.Join("",res);
        }
    }
}
