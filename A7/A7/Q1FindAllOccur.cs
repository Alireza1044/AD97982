using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q1FindAllOccur : Processor
    {
        public Q1FindAllOccur(string testDataName) : base(testDataName)
        {
			this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String, long[]>)Solve,"\n");

        public long[] Solve(string text, string pattern)
        {
            List<long> result = new List<long>();

            string textFinder = pattern + "$" + text;
            int[] borders = new int[textFinder.Length];
            borders[0] = 0;
            int pivot = 0;

            for (int i = 1; i < borders.Length; i++)
            {
                do
                {
                    if (textFinder[i] == textFinder[pivot])
                    {
                        pivot++;
                        borders[i] = pivot;
                        if (pivot == pattern.Length)
                        {
                            result.Add(i - (2 * pattern.Length));
                        }
                        break;
                    }
                    else
                    {
                        if(pivot > 0)
                            pivot = borders[pivot - 1];
                    }
                    if (pivot == 0 && textFinder[i] != textFinder[pivot])
                        break;
                } while (true);
            }

            if (result.Any())
                return result.ToArray();
            else
                return new long[] { -1 };
        }
    }
}
