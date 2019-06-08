using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace Exam2
{
    public class Q1LatinSquareSAT : Processor
    {
        public Q1LatinSquareSAT(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(37, 54);
            this.ExcludeTestCaseRangeInclusive(20, 54);
            //this.ExcludeTestCaseRangeInclusive(1, 3);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int?[,], string>)Solve);

        public override Action<string, string> Verifier =>
            TestTools.SatVerifier;


        public string Solve(int dim, int?[,] square)
        {
            string result = "";

            // exactly 1 digit
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    for (int k = 0; k < dim; k++)
                    {
                        result += $"{i * dim * dim + j * dim + 1 + k} ";
                    }
                    result += "\n ";
                }
                for (int j = 0; j < dim; j++)
                {
                    for (int k = 0; k < dim; k++)
                    {
                        for (int z = k + 1; z < dim; z++)
                        {
                            result += $"-{i * dim * dim + j * dim + k + 1} " +
                            $"-{i * dim * dim + j * dim + z + 1} \n ";
                        }
                    }
                }
            }


            // exactly 1 row
            for (int k = 0; k < dim; k++)
            {
                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        result += $"{i * dim * dim + j * dim + 1 + k} ";
                    }

                    result += "\n ";

                    for (int j = 0; j < dim; j++)
                    {
                        for (int z = j + 1; z < dim; z++)
                        {
                            result += $"-{i * dim * dim + j * dim + 1 + k} -{i * dim * dim + z * dim + 1 + k} \n ";
                        }
                    }
                }
            }

            // exactly 1 column
            for (int k = 0; k < dim; k++)
            {
                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        result += $"{j * dim * dim + i * dim + 1 + k} ";
                    }

                    result += "\n ";

                    for (int j = 0; j < dim; j++)
                    {
                        for (int z = j + 1; z < dim; z++)
                        {
                            result += $"-{j * dim * dim + i * dim + 1 + k} -{z * dim * dim + i * dim + 1 + k} \n ";
                        }
                    }
                }
            }

            // already in matrix
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (square[i, j].HasValue)
                        result += $"{i * dim * dim + j * dim + 1 + square[i, j].Value} \n ";
                }
            }

            result = result.Insert(0, $"{result.Split('\n').Count() - 1} {dim * dim * dim} \n ");

            return result;
        }
    }
}
