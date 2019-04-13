using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A6
{
    public class Q3MatchingAgainCompressedString : Processor
    {
        public Q3MatchingAgainCompressedString(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) => 
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        public long[] Solve(string text, long n, String[] patterns)
        {
            // write your code here
            List<long> result = new List<long>();
            List<(char, int)> lastColumn = new List<(char, int)>();
            for (int i = 0; i < text.Length; i++)
            {
                lastColumn.Add((text[i], i));
            }
            var firstColumn = lastColumn.OrderBy(x => x.Item1).ToList();
            for (int i = 0; i < firstColumn.Count; i++)
            {
                lastColumn[firstColumn[i].Item2] =
                    (lastColumn[firstColumn[i].Item2].Item1, i);
            }

            foreach (var pattern in patterns)
            {
                var charPattern = pattern.ToCharArray().ToList();
                int first = 0;
                int last = text.Length - 1;
                bool firstFound = true;
                for (int i = charPattern.Count - 1; i >= 0; i--) 
                {
                    if (!firstFound)
                        break;
                    firstFound = false;
                    int temp = last;
                    for (int j = first; j <= temp; j++)
                    {
                        if (!firstFound)
                        {
                            if (lastColumn[j].Item1 == charPattern[i])
                            {
                                first = lastColumn[j].Item2;
                                firstFound = true;
                                last = first;
                                continue;
                            }
                        }
                        else
                        {
                            if (lastColumn[j].Item1 == charPattern[i])
                                last = lastColumn[j].Item2;
                        }
                    }
                }
                if (firstFound)
                    result.Add(last - first + 1);
                else
                    result.Add(0);
            }
            return result.ToArray();
        }
    }
}
