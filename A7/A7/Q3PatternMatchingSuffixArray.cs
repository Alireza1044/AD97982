using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q3PatternMatchingSuffixArray : Processor
    {
        public Q3PatternMatchingSuffixArray(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, string[], long[]>)Solve, "\n");

        private long[] Solve(string text, long n, string[] patterns)
        {
            List<long> result = new List<long>();
            text = text + '$';
            long[] suffixArray = SuffixArray.BuildSuffixArray(text);
            bool[] isAdded = new bool[suffixArray.Length];
            foreach (var pattern in patterns)
            {
                int minIdx = LeftBS(suffixArray, pattern, text);
                int maxIdx = RightBS(suffixArray, pattern, text);
                if (minIdx == -1 || maxIdx == -1)
                {
                    continue;
                }
                for (int i = minIdx; i <= maxIdx; i++)
                {
                    if (!isAdded[i])
                    {
                        result.Add(suffixArray[i]);
                        isAdded[i] = true;
                    }
                }
            }
            if (!result.Any())
                result.Add(-1);
            return result.ToArray();
        }

        public int RightBS(long[] suffixArray, string pattern, string text)
        {
            int left = 0;
            int right = suffixArray.Length - 1;
            int middle;
            int minIdx = -1;
            while (left <= right)
            {
                middle = (left + right) / 2;
                switch (CompareTo(pattern, text, (int)suffixArray[middle]))
                {
                    case 1:
                        left = middle + 1;
                        break;
                    case 0:
                        minIdx = middle;
                        left = middle + 1;
                        break;
                    case -1:
                        right = middle - 1;
                        break;
                }
            }
            return minIdx;
        }
        public int LeftBS(long[] suffixArray, string pattern, string text)
        {
            int left = 0;
            int right = suffixArray.Length - 1;
            int middle;
            int maxIdx = -1;
            while (left <= right)
            {
                middle = (left + right) / 2;
                switch (CompareTo(pattern, text, (int)suffixArray[middle]))
                {
                    case 1:
                        left = middle + 1;
                        break;
                    case 0:
                        maxIdx = middle;
                        right = middle - 1;
                        break;
                    case -1:
                        right = middle - 1;
                        break;
                }
            }
            return maxIdx;
        }

        public int CompareTo(string pattern, string text, int subIndex)
        {
            int min = Math.Min(pattern.Length, text.Length - subIndex);
            int i = 0;

            for (; i < min; i++)
            {
                if (pattern[i] != text[i + subIndex])
                {
                    if (pattern[i] < text[i + subIndex])
                        return -1;
                    else return 1;
                }
            }

            if (i == pattern.Length)
                return 0;
            else return 1;
        }
    }
}
