using System;
using Microsoft.SolverFoundation.Solvers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestCommon;

namespace A3
{
    public class Q1FrequencyAssignment : Processor
    {
        public Q1FrequencyAssignment(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);

        public string[] Solve(int V, int E, long[,] matrix)
        {
            string[] result = new string[4 * V + 3 * E + 1];
            int clauseCount = 4 * V + 3 * E;
            int variableCount = V * 3;
            result[0] = clauseCount.ToString() + " " + variableCount.ToString();

            // first V clauses
            // 3 variable clauses
            for (int i = 1; i <= V; i++)
            {
                result[i] = $"{(3 * i - 2).ToString()} {(3 * i - 1).ToString()} {(3 * i).ToString()} 0";
            }

            // checking that just 1 variable in true for each node
            for (int i = V + 1; i <= 4 * V; i += 3)
            {
                result[i] = $"-{(i - V).ToString()} -{(i - V + 1).ToString()} 0";
                result[i + 1] = $"-{(i - V).ToString()}  -{(i - V + 2).ToString()} 0";
                result[i + 2] = $"-{(i - V + 1).ToString()} -{(i - V + 2).ToString()} 0";
            }

            // checking that neighbor nodes have different colors
            for (int i = 4 * V + 1, j = 0; i < result.Length; i += 3, j++)
            {
                result[i] = $"-{(3 * matrix[j, 0] - 2).ToString()} -{(3 * matrix[j, 1] - 2).ToString()} 0";
                result[i + 1] = $"-{(3 * matrix[j, 0] - 1).ToString()} -{(3 * matrix[j, 1] - 1).ToString()} 0"; ;
                result[i + 2] = $"-{(3 * matrix[j, 0]).ToString()} -{(3 * matrix[j, 1]).ToString()} 0";
            }

            return result;
        }

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;
    }
}
