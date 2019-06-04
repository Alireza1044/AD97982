using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q3AdBudgetAllocation : Processor
    {
        public Q3AdBudgetAllocation(string testDataName)
            : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(37, 45);
            //this.ExcludeTestCaseRangeInclusive(1, 36);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[], string[]>)Solve);

        public string[] Solve(long eqCount, long varCount, long[][] A, long[] b)
        {
            List<string> result = new List<string>();
            result.Add("First Line");

            List<long> constraint = new List<long>();

            for (int i = 0; i < A.Length; i++)
            {
                // add non-zero variables
                //constraint.Clear();

                //for (int j = 0; j < A[i].Length; j++)
                //{
                //    if (A[i][j] != 0)
                //    {
                //        constraint.Add(A[i][j]);
                //    }
                //}

                string temp = "";

                // check in the inequality
                // 3 variable
                if (varCount >= 3)
                {
                    for (int j = 0; j < varCount; j++)
                    {
                        for (int k = j + 1; k < varCount; k++)
                        {
                            for (int z = k + 1; z < varCount; z++)
                            {
                                temp = "";
                                // 1 1 1
                                if (A[i][j] * 1 + A[i][k] * 1 + A[i][z] * 1 > b[i])
                                {
                                    if (A[i][j] != 0)
                                        temp += $"{j + 1} ";

                                    if (A[i][k] != 0)
                                        temp += $"{k + 1} ";

                                    if (A[i][z] != 0)
                                        temp += $"{z + 1} ";

                                    if (temp != "")
                                    {
                                        temp += "0";
                                        result.Add(temp);
                                    }
                                }

                                temp = "";
                                // 1 1 0
                                if (A[i][j] * 1 + A[i][k] * 1 + A[i][z] * 0 > b[i])
                                {
                                    if (A[i][j] != 0)
                                        temp += $"{j + 1} ";

                                    if (A[i][k] != 0)
                                        temp += $"{k + 1} ";

                                    if (A[i][z] != 0)
                                        temp += $"-{z + 1} ";

                                    if (temp != "")
                                    {
                                        temp += "0";
                                        result.Add(temp);
                                    }
                                }

                                temp = "";
                                // 1 0 1
                                if (A[i][j] * 1 + A[i][k] * 0 + A[i][z] * 1 > b[i])
                                {
                                    if (A[i][j] != 0)
                                        temp += $"{j + 1} ";

                                    if (A[i][k] != 0)
                                        temp += $"-{k + 1} ";

                                    if (A[i][z] != 0)
                                        temp += $"{z + 1} ";

                                    if (temp != "")
                                    {
                                        temp += "0";
                                        result.Add(temp);
                                    }
                                }

                                temp = "";
                                // 1 0 0
                                if (A[i][j] * 1 + A[i][k] * 0 + A[i][z] * 0 > b[i])
                                {
                                    if (A[i][j] != 0)
                                        temp += $"{j + 1} ";

                                    if (A[i][k] != 0)
                                        temp += $"-{k + 1} ";

                                    if (A[i][z] != 0)
                                        temp += $"-{z + 1} ";

                                    if (temp != "")
                                    {
                                        temp += "0";
                                        result.Add(temp);
                                    }
                                }

                                temp = "";
                                // 0 1 1
                                if (A[i][j] * 0 + A[i][k] * 1 + A[i][z] * 1 > b[i])
                                {
                                    if (A[i][j] != 0)
                                        temp += $"-{j + 1} ";

                                    if (A[i][k] != 0)
                                        temp += $"{k + 1} ";

                                    if (A[i][z] != 0)
                                        temp += $"{z + 1} ";

                                    if (temp != "")
                                    {
                                        temp += "0";
                                        result.Add(temp);
                                    }
                                }

                                temp = "";
                                // 0 1 0
                                if (A[i][j] * 0 + A[i][k] * 1 + A[i][z] * 0 > b[i])
                                {
                                    if (A[i][j] != 0)
                                        temp += $"-{j + 1} ";

                                    if (A[i][k] != 0)
                                        temp += $"{k + 1} ";

                                    if (A[i][z] != 0)
                                        temp += $"-{z + 1} ";

                                    if (temp != "")
                                    {
                                        temp += "0";
                                        result.Add(temp);
                                    }
                                }

                                temp = "";
                                // 0 0 1
                                if (A[i][j] * 0 + A[i][k] * 0 + A[i][z] * 1 > b[i])
                                {
                                    if (A[i][j] != 0)
                                        temp += $"-{j + 1} ";

                                    if (A[i][k] != 0)
                                        temp += $"-{k + 1} ";

                                    if (A[i][z] != 0)
                                        temp += $"{z + 1} ";

                                    if (temp != "")
                                    {
                                        temp += "0";
                                        result.Add(temp);
                                    }
                                }

                                temp = "";
                                // 0 0 0
                                if (A[i][j] * 0 + A[i][k] * 0 + A[i][z] * 0 > b[i])
                                {
                                    if (A[i][j] != 0)
                                        temp += $"-{j + 1} ";

                                    if (A[i][k] != 0)
                                        temp += $"-{k + 1} ";

                                    if (A[i][z] != 0)
                                        temp += $"-{z + 1} ";

                                    if (temp != "")
                                    {
                                        temp += "0";
                                        result.Add(temp);
                                    }
                                }
                            }
                        }
                    }
                }
                
                // 2 variable
                else if(varCount == 2)
                {
                    for (int j = 0; j < varCount; j++)
                    {
                        for (int k = j+1; k < varCount; k++)
                        {
                            temp = "";
                            // 1 1
                            if(A[i][j] * 1 + A[i][k] * 1 > b[i])
                            {
                                if (A[i][j] != 0)
                                    temp += $"{j + 1} ";

                                if (A[i][k] != 0)
                                    temp += $"{k + 1} ";

                                if(temp != "")
                                {
                                    temp += "0";
                                    result.Add(temp);
                                }
                            }

                            temp = "";
                            // 1 0
                            if (A[i][j] * 1 + A[i][k] * 0 > b[i])
                            {
                                if (A[i][j] != 0)
                                    temp += $"{j + 1} ";

                                if (A[i][k] != 0)
                                    temp += $"-{k + 1} ";

                                if (temp != "")
                                {
                                    temp += "0";
                                    result.Add(temp);
                                }
                            }

                            temp = "";
                            // 0 1
                            if (A[i][j] * 0 + A[i][k] * 1 > b[i])
                            {
                                if (A[i][j] != 0)
                                    temp += $"-{j + 1} ";

                                if (A[i][k] != 0)
                                    temp += $"{k + 1} ";

                                if (temp != "")
                                {
                                    temp += "0";
                                    result.Add(temp);
                                }
                            }

                            temp = "";
                            // 0 0
                            if (A[i][j] * 0 + A[i][k] * 0 > b[i])
                            {
                                if (A[i][j] != 0)
                                    temp += $"-{j + 1} ";

                                if (A[i][k] != 0)
                                    temp += $"-{k + 1} ";

                                if (temp != "")
                                {
                                    temp += "0";
                                    result.Add(temp);
                                }
                            }
                        }
                    }
                }

                // 1 variable
                else if(varCount == 1)
                {
                    for (int j = 0; j < varCount; j++)
                    {
                        temp = "";
                        // 1
                        if(A[i][j] * 1 > b[i])
                        {
                            if (A[i][j] != 0)
                                temp += $"{j + 1} ";

                            if(temp != "")
                            {
                                temp += "0";
                                result.Add(temp);
                            }
                        }

                        temp = "";
                        // 0
                        if (A[i][j] * 0 > b[i])
                        {
                            if (A[i][j] != 0)
                                temp += $"-{j + 1} ";

                            if (temp != "")
                            {
                                temp += "0";
                                result.Add(temp);
                            }
                        }
                    }
                }
            }

            result[0] = $"{result.Count - 1} {varCount}";

            return result.ToArray();
        }

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;
    }
}
