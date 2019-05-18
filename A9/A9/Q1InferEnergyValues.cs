using System;
using TestCommon;

namespace A9
{
    public class Q1InferEnergyValues : Processor
    {
        public Q1InferEnergyValues(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(1, 13);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, double[,], double[]>)Solve);

        public double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            return GaussianElim(MATRIX_SIZE, matrix);
        }



        private double[] GaussianElim(long matrixSize, double[,] matrix)
        {

            // n - 1 = matrixSize - 2
            // n = matrixSize - 1
            // n + 1 = matrixSize

            //for (int i = 0; i <= matrixSize - 2; i++)
            //{
            //    if (matrix[i, i] == 0)
            //        continue;

            //    for (int j = i + 1; j <= matrixSize - 1; j++)
            //    {
            //        for (int k = 0; k <= matrixSize; k++)
            //        {
            //            matrix[j, k] -= (matrix[j, i] / matrix[i, i]) * matrix[i, k];
            //        }
            //    }
            //}

            //result[matrixSize - 1] = matrix[matrixSize - 1, matrixSize] / matrix[matrixSize, matrixSize];           

            //for (int i = (int)matrixSize - 2; i >= 0; i--)
            //{
            //    result[i] = matrix[i, matrixSize];
            //    for (int j = i + 1; j < matrixSize; j++)
            //    {
            //        result[i] -= matrix[i, j] * result[j];
            //    }
            //    result[i] /= matrix[i, i];
            //}

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
                //result[i] = Math.Floor(result[i]);
            }
            return result;
        }

        //private double[] Backwards(double[,] matrix, long matrixSize)
        //{
        //    double[] result = new double[matrixSize];

        //    for (int i = (int)matrixSize - 1; i >= 0; i--)
        //    {
        //        result[i] = matrix[i, matrixSize];

        //        for (int j = i + 1; j < matrixSize; j++)
        //        {
        //            result[i] -= matrix[i, j] * result[j];
        //        }
        //        result[i] /= matrix[i, i];
        //    }
        //    for (int i = 0; i < result.Length; i++)
        //    {
        //        result[i] = Math.Round(result[i]);
        //    }
        //    return result;
        //}

        //private void ForwardElim(double[,] matrix, long matrixSize)
        //{
        //    for (int i = 0; i < matrixSize; i++)
        //    {
        //        int r_max = i;
        //        int c_max = (int)matrix[r_max, i];

        //        for (int j = i + 1; j < matrixSize; j++)
        //        {
        //            if (Math.Abs(matrix[i, j]) > c_max)
        //            {
        //                c_max = (int)matrix[i, j];
        //                r_max = j;
        //            }
        //        }

        //        if (matrix[i, r_max] == 0)
        //            return;

        //        if (r_max != i)
        //            SwapRows(matrix, matrixSize, i, r_max);

        //        for (int j = i + 1; j < matrixSize; j++)
        //        {
        //            double quotient = matrix[j, i] / matrix[i, i];

        //            for (int k = 0; k <= matrixSize; k++)
        //            {
        //                matrix[j, k] -= matrix[i, j] * quotient;
        //            }
        //            matrix[j, i] = 0;
        //        }
        //    }
        //}

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