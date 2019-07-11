using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Solvers;
using TestCommon;

namespace A11
{
    public class Q4RescheduleExam : Processor
    {
        public Q4RescheduleExam(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, char[], long[][], char[]>)Solve);

        public static readonly char[] colors_3 = new char[] { 'R', 'G', 'B' };
        public static readonly Dictionary<char, long> colors_num = new Dictionary<char, long>
        {
            {'R', 1 },
            {'G', 2 },
            {'B', 3 }

        };
        // R = 1
        // G = 2
        // B = 3

        public override Action<string, string> Verifier =>
            TestTools.GraphColorVerifier;


        public virtual char[] Solve(long nodeCount, char[] colors, long[][] edges)
        {
            long variableCount = 3 * nodeCount;

            List<long[]> cnf = new List<long[]>();

            for (int i = 0; i < colors.Length; i++)
            {
                long temp = 0;
                switch (colors[i])
                {
                    case 'R':
                        temp = i * 3 + 1;
                        cnf.Add(new long[] { temp + 1, temp + 2 });
                        cnf.Add(new long[] { -1 * (temp + 1), -1 * (temp + 2) });
                        break;
                    case 'G':
                        temp = i * 3 + 2;
                        cnf.Add(new long[] { temp - 1, temp + 1 });
                        cnf.Add(new long[] { -1 * (temp - 1), -1 * (temp + 1) });
                        break;
                    case 'B':
                        temp = i * 3 + 3;
                        cnf.Add(new long[] { temp - 1, temp - 2 });
                        cnf.Add(new long[] { -1 * (temp - 1), -1 * (temp - 2) });
                        break;
                }

                cnf.Add(new long[] { temp * -1, temp * -1 });
            }

            for (int i = 0; i < edges.Length; i++)
            {
                var fColor = colors[edges[i][0] - 1];
                var sColor = colors[edges[i][1] - 1];

                cnf.Add(new long[] { -1 * (edges[i][0] * 3 - 2), -1 * (edges[i][1] * 3 - 2) });
                cnf.Add(new long[] { -1 * (edges[i][0] * 3 - 1), -1 * (edges[i][1] * 3 - 1) });
                cnf.Add(new long[] { -1 * (edges[i][0] * 3), -1 * (edges[i][1] * 3) });

            }
            var q1 = new Q1CircuitDesign("TD4");

            var satResult = q1.Solve(nodeCount * 3, cnf.Count, cnf.ToArray());

            char[] result = new char[nodeCount];

            if (satResult.Item1 == false)
                return "Impossible".ToCharArray();
            else
            {
                var answers = satResult.Item2;
                for (int i = 0; i < answers.Length; i++)
                {
                    if (answers[i] > 0)
                    {
                        var temp = answers[i] % 3;

                        switch (temp)
                        {
                            case 1:
                                result[(answers[i] - 1) / 3] = 'R';
                                break;
                            case 2:
                                result[(answers[i] - 1) / 3] = 'G';
                                break;
                            case 0:
                                result[(answers[i] - 1) / 3] = 'B';
                                break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
