using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A9
{
    public class Q3OnlineAdAllocation : Processor
    {

        public Q3OnlineAdAllocation(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCases(new int[] { 5, 33, 41 });
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, double[,], String>)Solve);

        public string Solve(int c, int v, double[,] matrix1)
        {
            double[,] matrix = BuildMatrix(matrix1, c, v);
            var pivot = FindPivot(matrix);

            while (pivot.Item2 != -1)
            {
                RowFunctions(matrix, pivot);
                pivot = FindPivot(matrix);

                if (pivot.Item1 == -1)
                    return "Infinity";
            }

            return BackTrack(matrix, matrix1, c, v);
        }

        private string BackTrack(double[,] matrix, double[,] matrix1, int c, int v)
        {
            string result = "Bounded Solution\n";
            double[] res = new double[v];
            bool flag;
            for (int i = 0; i < v; i++)
            {
                flag = false;

                for (int j = 0; j < c; j++)
                {
                    if (matrix[j, i] == 1 && !flag)
                    {
                        res[i] = matrix[j, matrix.GetLength(1) - 1];
                        flag = true;
                    }
                    else if (matrix[j, i] != 0 && flag)
                    {
                        res[i] = 0;
                        break;
                    }
                }
            }

            double temp = 0;

            for (int i = 0; i < matrix1.GetLength(0) - 1; i++)
            {
                temp = 0;
                for (int j = 0; j < matrix1.GetLength(1) - 1; j++)
                {
                    temp += matrix1[i, j] * res[j];
                }
                temp = Math.Round(temp * 2) / 2;
                if (temp > matrix1[i, matrix1.GetLength(1) - 1])
                    return "No Solution";
            }

            //generating result
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = Math.Round(res[i] * 2) / 2;
                result = result + res[i] + " ";
            }



            return result.TrimEnd(' ');
        }

        private void RowFunctions(double[,] matrix, Tuple<int, int> pivot)
        {
            // <row,column>
            double temp = int.MaxValue;
            int row = -1;

            //for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            //{
            //    if (i == pivot.Item1)
            //        continue;

            //    if((matrix[i, pivot.Item2]/matrix[i,matrix.GetLength(0)-1]) < temp)
            //    {
            //        row = i;
            //        temp = matrix[i, pivot.Item2] / matrix[i, matrix.GetLength(0) - 1];
            //    }
            //}

            double quotient = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (i == pivot.Item1)
                    continue;

                quotient = matrix[i, pivot.Item2] / matrix[pivot.Item1, pivot.Item2] * -1;

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] += (matrix[pivot.Item1, j] * quotient);
                }
            }
        }

        private Tuple<int, int> FindPivot(double[,] matrix)
        {
            int column = -1, row = -1;
            double temp = int.MaxValue;
            for (int i = 0; i < matrix.GetLength(1) - 1; i++)
            {
                if (matrix[matrix.GetLength(0) - 1, i] < 0 && matrix[matrix.GetLength(0) - 1, i] < temp)
                {
                    temp = matrix[matrix.GetLength(0) - 1, i];
                    column = i;
                }
            }

            if (column == -1)
                return new Tuple<int, int>(0, -1);

            temp = int.MaxValue;

            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                if (matrix[i, column] == 0
                    || matrix[i, matrix.GetLength(1) - 1] / matrix[i, column] < 0)
                    continue;

                if (matrix[i, matrix.GetLength(1) - 1] / matrix[i, column] < temp)
                {
                    row = i;
                    temp = matrix[i, matrix.GetLength(1) - 1] / matrix[i, column];
                }
            }

            if (row == -1)
                return new Tuple<int, int>(-1, 0);

            temp = matrix[row, matrix.GetLength(1) - 1] / matrix[row, column];

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (i == column)
                    continue;
                matrix[row, i] /= matrix[row, column];
            }
            matrix[row, column] = 1;
            return new Tuple<int, int>(row, column);
        }

        private double[,] BuildMatrix(double[,] matrix1, int row, int column)
        {
            double[,] matrix = new double[row + 1, column + row + 2];

            for (int i = 0; i < row + 1; i++)
            {
                matrix[i, i + column] = 1;
                for (int j = 0; j < column; j++)
                {
                    matrix[i, j] = matrix1[i, j];
                }
                matrix[i, matrix.GetLength(1) - 1] = matrix1[i, matrix1.GetLength(1) - 1];
            }

            for (int i = 0; i < column; i++)
            {
                matrix[matrix.GetLength(0) - 1, i] *= -1;
            }

            return matrix;
        }

        private void SwapRows(double[,] matrix, int firstRow, int secondRow)
        {
            double temp;
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                temp = matrix[firstRow, i];
                matrix[firstRow, i] = matrix[secondRow, i];
                matrix[secondRow, i] = temp;
            }
        }
    }
}
