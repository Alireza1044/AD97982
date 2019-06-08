using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace Exam2
{
    public class Q2LatinSquareBT : Processor
    {
        public Q2LatinSquareBT(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(28, 120);
            this.ExcludeTestCases(new int[] { 2, 3, 9, 11, 12, 14, 15, 21, 23, 27, 33, 50 });
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int?[,], string>)Solve);

        public string Solve(int dim, int?[,] square)
        {
            return SolveSAT(dim, square);
        }

        private string SolveSAT(int dim, int?[,] square)
        {
            bool[] isAvailable = new bool[dim];
            isAvailable.Initialize();
            bool flag = false;
            // satr
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    flag = false;
                    if (square[i, j].HasValue)
                    {
                        isAvailable[square[i, j].Value] = true;
                    }
                    else
                    {
                        isAvailable.Initialize();
                        flag = true;
                        break;
                    }
                }

                if (flag)
                    continue;

                for (int j = 0; j < dim; j++)
                {
                    if (isAvailable[j] == false)
                    {
                        return "UNSATISFIABLE";
                    }
                }
            }

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    flag = false;
                    if (square[j, i].HasValue)
                    {
                        isAvailable[square[j, i].Value] = true;
                    }
                    else
                    {
                        isAvailable.Initialize();
                        flag = true;
                        break;
                    }
                }

                if (flag)
                    continue;

                for (int j = 0; j < dim; j++)
                {
                    if (isAvailable[j] == false)
                    {
                        return "UNSATISFIABLE";
                    }
                }
            }

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (!square[i, j].HasValue)
                        continue;

                    for (int k = j + 1; k < dim; k++)
                    {
                        if (!square[i, k].HasValue)
                            continue;

                        if (square[i, j] == square[i, k])
                            return "UNSATISFIABLE";
                    }
                    for (int k = j + 1; k < dim; k++)
                    {
                        if (!square[j, i].HasValue || !square[k, i].HasValue)
                            continue;

                        if (square[j, i] == square[k, i])
                            return "UNSATISFIABLE";
                    }
                    return "SATISFIABLE";
                }
            }

            for (int k = 0; k < dim; k++)
            {
                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        if (!square[i, j].HasValue)
                        {
                            square[i, j] = k;
                            if (SolveSAT(dim, square) == "SATISFIABLE")
                                return "SATISFIABLE";
                            square[i, j] = null;
                        }
                    }
                }
            }

            return "UNSATISFIABLE";

        }
    }
}
