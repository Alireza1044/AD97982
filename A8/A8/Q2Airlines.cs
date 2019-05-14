using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q2Airlines : Processor
    {
        public Q2Airlines(string testDataName) : base(testDataName)
        {
            //this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[]>)Solve);

        public virtual long[] Solve(long flightCount, long crewCount, long[][] info)
        {
            int[,] graph = new int[flightCount + crewCount + 2, flightCount + crewCount + 2];
            int[,] residualGraph = new int[flightCount + crewCount + 2, flightCount + crewCount + 2];

            BuildNetwork(flightCount, crewCount, info, graph);
            BuildNetwork(flightCount, crewCount, info, residualGraph);
            FindMaxFlow(graph, residualGraph, flightCount, crewCount);
            return BipartiteMatch(residualGraph, flightCount, crewCount);
        }

        private long[] BipartiteMatch(int[,] residualGraph, long m, long n)
        {
            long[] result = new long[m];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = -1;
            }

            for (int i = (int)m + 1; i < m + n + 1; i++)
            {
                for (int j = 1; j <= m+1; j++)
                {
                    if (residualGraph[i, j] != 0)
                    {
                        result[j - 1] = i - m;
                    }
                }
            }
            return result;
        }

        private void BuildNetwork(long n, long m, long[][] info, int[,] graph)
        {
            for (int i = 1; i <= n; i++)
            {
                graph[0, i] = 1;
            }

            for (int i = (int)n + 1; i <= m + n; i++)
            {
                graph[i, m + n + 1] = 1;
            }

            for (int i = 0; i < info.Length; i++)
            {
                for (int j = 0; j < info[i].Length; j++)
                {
                    if (info[i][j] == 1)
                    {
                        graph[i + 1, j + n + 1] = 1;
                    }
                }
            }
        }

        internal static void FindMaxFlow(int[,] graph, int[,] residualGraph, long m, long n)
        {
            int source = 0;
            int sink = (int)(m + n + 1);
            int[] parent = new int[m + n + 2];
            long maxFlow = 0;

            while (BFS(residualGraph, source, sink, parent, m + n + 2))
            {
                long pathFlow = int.MaxValue;
                int i = sink;

                while (i != source && i != 0)
                {
                    pathFlow = Math.Min(pathFlow, residualGraph[parent[i], i]);
                    i = parent[i];
                }

                i = sink;

                while (i != source && i != 0)
                {
                    residualGraph[parent[i], i] -= (int)pathFlow;
                    residualGraph[i, parent[i]] += (int)pathFlow;
                    i = parent[i];
                }

                maxFlow += pathFlow;
                if (pathFlow == 0) break;
            }
        }

        private static bool BFS(int[,] residualGraph, int source, int sink, int[] parent, long nodeCount)
        {
            bool[] isVisited = new bool[nodeCount];

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);
            isVisited[source] = true;
            parent[source] = -1;

            while (queue.Count != 0)
            {
                int temp = queue.Dequeue();
                for (int i = 0; i < nodeCount; i++)
                {
                    if (isVisited[i] == false && residualGraph[temp, i] > 0)
                    {
                        queue.Enqueue(i);
                        parent[i] = temp;
                        isVisited[i] = true;
                    }
                }
            }

            return isVisited[source] == true;
        }

        //public static long[] MaximumMatching(long m, long n, long[][] info)
        //{
        //    long[] result = new long[m];

        //    long[] matchR = new long[m];

        //    for (int i = 0; i < m; i++)
        //    {
        //        matchR[i] = -1;
        //    }

        //    for (int i = 0; i < m; i++)
        //    {
        //        bool[] isVisited = new bool[n];
        //        Matching(info, i, isVisited, matchR, result, n, m);
        //    }
        //    return matchR;
        //}

        //private static bool Matching(long[][] info, int i, bool[] isVisited,
        //    long[] matchR, long[] result, long n, long m)
        //{
        //    for (int j = 0; j < n; j++)
        //    {
        //        if (info[i][j] == 1 && !isVisited[j])
        //        {
        //            isVisited[j] = true;
        //            if (matchR[j] < 0 || Matching(info, (int)matchR[j], isVisited, matchR, result, n, m))
        //            {
        //                matchR[j] = i;
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}
