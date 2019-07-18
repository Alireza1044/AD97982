using System;
using TestCommon;

namespace A9
{
    public class Q1InferEnergyValues : Processor
    {
        public Q1InferEnergyValues(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, double[,], double[]>)Solve);

        public double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            return GaussianElim(MATRIX_SIZE, matrix);
        }



        private double[] GaussianElim(long matrixSize, double[,] matrix)
        {
            double[] result = new double[matrixSize];
            int maxRow = 0;
            double max, temp;

            for (int i = 0; i < matrixSize; i++)
            {
                maxRow = i;
                max = (int)matrix[i, i];

                for (int j = i + 1; j < matrixSize; j++)
                {
                    if (max == 0 && matrix[j, i] != 0)
                    {
                        SwapRows(matrix, matrixSize, i, j);
                    }
                    if ((temp = Math.Abs(matrix[j,i])) > max)
                    {
                        maxRow = j;
                        max = temp;
                    }
                }

                if(max != 0)
                    SwapRows(matrix, matrixSize, maxRow, i);

                for (int j = i + 1; j < matrixSize; j++)
                {
                    temp = matrix[j, i] / matrix[i, i];
                    for (int k = i + 1; k < matrixSize; k++)
                    {
                        matrix[j, k] -= temp * matrix[i, k];
                    }
                    matrix[j, i] = 0;
                    matrix[j, matrixSize] -= temp * matrix[i, matrixSize];
                }
            }

            for (int i = (int)matrixSize - 1; i >= 0; i--)
            {
                temp = matrix[i, matrixSize];
                for (int j = (int)matrixSize - 1; j > i; j--)
                {
                    temp -= result[j] * matrix[i, j];
                }
                result[i] = temp / matrix[i, i];
            }

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Round(result[i] * 2,0) / 2;
            }

            return result;
        }

        private void SwapRows(double[,] matrix, long matrixSize, long firstRow, long secondRow)
        {
            for (int i = 0; i <= matrixSize; i++)
            {
                var temp = matrix[secondRow, i];
                matrix[secondRow, i] = matrix[firstRow, i];
                matrix[firstRow, i] = temp;
            }
        }
    }
}