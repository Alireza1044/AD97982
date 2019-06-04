using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q2CleaningApartment : Processor
    {
        public Q2CleaningApartment(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);

        public string[] Solve(int V, int E, long[,] matrix)
        {
            int variableCount = V * V;
            List<string> result = new List<string>();

            result.Add("first line");

            string temp = "";

            //// 1v2v3v...vV satri
            //for (int i = 0; i < V; i++)
            //{
            //    temp = "";
            //    for (int j = 0; j < V; j++)
            //    {
            //        temp += $"{i * V + j + 1} ";
            //    }
            //    temp += "0";
            //    result.Add(temp);
            //}

            // satri 2 taii
            for (int i = 0; i < V; i++)
            {
                for (int j = i * V + 1; j <= (i + 1) * V; j++)
                {
                    for (int k = j + 1; k <= (i + 1) * V; k++)
                    {
                        temp = $"-{j} -{k} 0";
                        result.Add(temp);
                    }
                }
            }

            //// -1v-2v-3v...v-V satri
            //for (int i = 0; i < V; i++)
            //{
            //    for (int j = 0; j < V; j++)
            //    {
            //        temp = "";
            //        for (int k = 0; k < V; k++)
            //        {
            //            if (k == j)
            //                continue;
            //            temp += $"-{i * V + k + 1} ";
            //        }
            //        temp += "0";
            //        result.Add(temp);
            //    }
            //}

            //// sotooni
            //for (int i = 0; i < V; i++)
            //{
            //    temp = "";
            //    for (int j = 0; j < V; j++)
            //    {
            //        temp += $"{j * V + 1 + i} ";
            //    }
            //    temp += "0";
            //    result.Add(temp);
            //}

            // sotooni 2 taii
            for (int i = 0; i < V; i++)
            {
                for (int j = i + 1; j < V * V; j += V)
                {
                    for (int k = j + V; k <= V * V; k += V)
                    {
                        temp = $"-{j} -{k} 0";
                        result.Add(temp);
                    }
                }
            }

            //// sotooni
            //for (int i = 0; i < V; i++)
            //{
            //    for (int j = 0; j < V; j++)
            //    {
            //        temp = "";
            //        for (int k = 0; k < V; k++)
            //        {
            //            if (k == j)
            //                continue;
            //            temp += $"-{k * V + i + 1} ";
            //        }
            //        temp += "0";
            //        result.Add(temp);
            //    }
            //}

            //build reverse adjacency graph
            int[,] revAdjMatrix = new int[V + 1, V + 1];
            for (int i = 1; i <= V; i++)
            {
                for (int j = 1; j <= V; j++)
                {
                    revAdjMatrix[i, j] = 1;
                }
            }

            for (int i = 0; i < E; i++)
            {
                revAdjMatrix[matrix[i, 0], matrix[i, 1]] = 0;
                revAdjMatrix[matrix[i, 1], matrix[i, 0]] = 0;
            }

            for (int i = 0; i < V + 1; i++)
            {
                revAdjMatrix[i, i] = 0;
            }

            // check adjacency
            for (int i = 1; i <= V; i++)
            {
                for (int j = 1; j <= V; j++)
                {
                    if (revAdjMatrix[i, j] == 1)
                    {
                        temp = "";
                        for (int k = 1; k < V; k++)
                        {
                            temp = $"-{V * i - k} -{V * j - k + 1} 0";
                            result.Add(temp);
                            temp = $"-{V * i - k + 1} -{V * j - k} 0";
                            result.Add(temp);
                        }
                    }
                }
            }

            result[0] = $"{result.Count - 1} {V * V}";

            return result.ToArray();
        }

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;
    }
}
