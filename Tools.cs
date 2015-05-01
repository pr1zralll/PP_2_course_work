using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP_2_course_work
{
    class Tools
    {
        public static int[] MultiVecToMatrix(int[] V, int[][] MA)
        {
            int[] res = new int[V.Length];
                for (int i = 0; i < V.Length; i++)
                    for (int j = 0; j < V.Length; j++)
                        res[i] += V[j] * MA[i][j];
            return res;
        }
        public static int[] SumVectors(int[] A, int[] B)
        {
            int[] res = new int[A.Length];
                for (int i = 0; i < A.Length; i++)
                    res[i] = A[i] + B[i];
            return res;
        }
        public static void print(int[][] MA)
        {
            for (int i = 0; i < MA.Length; i++)
            {
                for (int j = 0; j < MA.Length; j++)
                    Console.Write(MA[i][j]+" ");
                Console.WriteLine();
            }
        }
        public static void print(int[] V)
        {
             for (int i = 0; i < V.Length; i++)
                 Console.Write(V[i] + " ");
        }
        public static int[] MultiNumVec(int a, int[] B)
        {
            int[] res = new int[B.Length];
            for (int i = 0; i < B.Length; i++)
                res[i] = B[i]*a;
            return res;
        }
        public static int[][] generateMatrix(int value, int Length)
        {
            int[][] res = new int[Length][];
            for (int i = 0; i < Length; i++)
            {
                res[i] = new int[Length];
                for (int j = 0; j < Length; j++)
                    res[i][j] = value;
            }
            return res;
        }
    }
}
