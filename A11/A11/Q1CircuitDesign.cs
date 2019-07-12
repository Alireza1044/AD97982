using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;
using Microsoft.SolverFoundation.Solvers;

namespace A11
{
    public class Q1CircuitDesign : Processor
    {
        public Q1CircuitDesign(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], Tuple<bool, long[]>>)Solve);

        public override Action<string, string> Verifier =>
            TestTools.SatAssignmentVerifier;

        Stack<long> S;
        List<List<long>> SCCs;

        public virtual Tuple<bool, long[]> Solve(long v, long c, long[][] cnf)
        {
            Edge[] graph = new Edge[2 * v + 1];

            S = new Stack<long>();
            SCCs = new List<List<long>>();
            for (int i = 1; i < graph.Length; i++)
            {
                graph[i] = new Edge(i);
            }
            //build the graph
            for (int i = 0; i < cnf.Length; i++)
            {
                if (cnf[i][0] < 0)
                {
                    if (cnf[i][1] < 0)
                    {
                        // -1 -3
                        graph[Math.Abs(cnf[i][0])].Connected.Add(Math.Abs(cnf[i][1]) + v); // 1 => -3
                        graph[Math.Abs(cnf[i][1])].Connected.Add(Math.Abs(cnf[i][0]) + v); // 3 => -1
                        // reverse
                        graph[Math.Abs(cnf[i][1]) + v].ConnectedReverse.Add(Math.Abs(cnf[i][0]));
                        graph[Math.Abs(cnf[i][0]) + v].ConnectedReverse.Add(Math.Abs(cnf[i][1]));
                    }
                    else
                    {
                        // -1 3
                        graph[Math.Abs(cnf[i][0])].Connected.Add(Math.Abs(cnf[i][1])); // 1 => 3
                        graph[Math.Abs(cnf[i][1]) + v].Connected.Add(Math.Abs(cnf[i][0]) + v); // -3 => -1
                        // reverse
                        graph[Math.Abs(cnf[i][1])].ConnectedReverse.Add(Math.Abs(cnf[i][0]));
                        graph[Math.Abs(cnf[i][0]) + v].ConnectedReverse.Add(Math.Abs(cnf[i][1]) + v);
                    }
                }
                else
                {
                    if (cnf[i][1] < 0)
                    {
                        // 1 -3
                        graph[Math.Abs(cnf[i][0]) + v].Connected.Add(Math.Abs(cnf[i][1]) + v); // -1 => -3
                        graph[Math.Abs(cnf[i][1])].Connected.Add(Math.Abs(cnf[i][0])); // 3 => 1
                        // reverse
                        graph[Math.Abs(cnf[i][1]) + v].ConnectedReverse.Add(Math.Abs(cnf[i][0]) + v);
                        graph[Math.Abs(cnf[i][0])].ConnectedReverse.Add(Math.Abs(cnf[i][1]));
                    }
                    else
                    {
                        // 1 3
                        graph[Math.Abs(cnf[i][0]) + v].Connected.Add(Math.Abs(cnf[i][1])); // -1 => 3
                        graph[Math.Abs(cnf[i][1]) + v].Connected.Add(Math.Abs(cnf[i][0])); // -3 => 1
                        // reverse
                        graph[Math.Abs(cnf[i][1])].ConnectedReverse.Add(Math.Abs(cnf[i][0]) + v);
                        graph[Math.Abs(cnf[i][0])].ConnectedReverse.Add(Math.Abs(cnf[i][1]) + v);
                    }
                }
            }

            for (int i = 1; i < graph.Length; i++)
            {
                if (!graph[i].IsVisited)
                {
                    DFS(graph, cnf, i);
                }
            }
            int result = 0;
            while (S.Count > 0)
            {
                long temp = S.Pop();
                if (graph[temp].IsVisited)
                {
                    ReverseDFS(graph, cnf, temp);
                    result++;
                }
            }

            bool isSat = true;

            isSat = CheckSat(v);


            if (!isSat)
                return new Tuple<bool, long[]>(false, null);

            long[] answers = new long[v];

            SolveCNF(answers,v);
        
            return new Tuple<bool, long[]>(true,answers);
        }

        private void SolveCNF(long[] answers, long v)
        {
            for (int i = 0; i < SCCs.Count; i++)
            {
                for (int j = 0; j < SCCs[i].Count; j++)
                {
                    if (SCCs[i][j] > v)
                    {
                        if (answers[SCCs[i][j] - v - 1] == 0)
                            answers[SCCs[i][j] - v - 1] = (SCCs[i][j] - v);
                    }
                    else
                    {
                        if (answers[SCCs[i][j] - 1] == 0)
                            answers[SCCs[i][j] - 1] = SCCs[i][j] * -1;
                    }
                }
            }
        }

        private bool CheckSat(long v)
        {
            bool[] inSCC = new bool[v + 1];

            for (int i = 0; i < SCCs.Count; i++)
            {
                if (SCCs[i].Count == 1)
                    continue;
                for (int j = 0; j < inSCC.Length; j++)
                {
                    inSCC[j] = false;
                }
                for (int j = 0; j < SCCs[i].Count; j++)
                {
                    if (SCCs[i][j] > v)
                    {
                        if (inSCC[SCCs[i][j] - v])
                            return false;
                        else
                            inSCC[SCCs[i][j] - v] = true;
                    }
                    else
                    {
                        if (inSCC[SCCs[i][j]])
                            return false;
                        else
                            inSCC[SCCs[i][j]] = true;
                    }
                }
            }

            return true;
        }

        public void DFS(Edge[] graph, long[][] edges, long startnode)
        {
            Stack<long> stack = new Stack<long>();
            stack.Push(startnode);
            while (stack.Count > 0)
            {
                long temp = stack.Pop();
                graph[temp].IsVisited = true;
                for (int i = 0; i < graph[temp].Connected.Count; i++)
                {
                    if (!graph[graph[temp].Connected[i]].IsVisited)
                    {
                        DFS(graph, edges, graph[temp].Connected[i]);
                    }
                }
                S.Push(temp);
            }
        }

        public void ReverseDFS(Edge[] graph, long[][] edges, long startnode)
        {
            Stack<long> stack = new Stack<long>();
            stack.Push(startnode);
            List<long> SCC = new List<long>();
            while (stack.Count > 0)
            {
                long temp = stack.Pop();
                graph[temp].IsVisited = false;
                SCC.Add(temp);
                for (int i = 0; i < graph[temp].ConnectedReverse.Count; i++)
                {
                    if (graph[graph[temp].ConnectedReverse[i]].IsVisited)
                    {
                        stack.Push(graph[temp].ConnectedReverse[i]);
                        graph[graph[temp].ConnectedReverse[i]].IsVisited = false;
                    }
                }
            }
            SCCs.Add(SCC);
        }
    }
}
