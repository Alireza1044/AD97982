using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q3Stocks : Processor
    {
        public Q3Stocks(string testDataName) : base(testDataName)
        { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long stockCount, long pointCount, long[][] matrix)
        {
            var info = Compare(stockCount, pointCount, matrix);

            int[,] graph = new int[stockCount + stockCount + 2, stockCount + stockCount + 2];
            int[,] residualGraph = new int[stockCount + stockCount + 2, stockCount + stockCount + 2];

            Q2Airlines.BuildNetwork(stockCount, stockCount, info, graph);
            Q2Airlines.BuildNetwork(stockCount, stockCount, info, residualGraph);
            var maxFlow = Q2Airlines.FindMaxFlow(graph, residualGraph, stockCount, stockCount);
            return stockCount - maxFlow;
        }

        private long[][] Compare(long stockCount, long pointCount, long[][] matrix)
        {
            long[][] result = new long[stockCount][];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new long[stockCount];
            }

            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < result[i].Length; j++)
                {
                    result[i][j] = 1;
                }
            }

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int k = 0; k < matrix.Length; k++)
                {
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        if(matrix[i][j] >= matrix[k][j])
                        {
                            result[i][k] = 0;
                        }
                    }
                }
            }

            return result;
        }
    }
}
