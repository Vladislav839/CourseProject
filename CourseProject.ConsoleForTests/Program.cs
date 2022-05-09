using System;

using CourseProject.BusinessLogic.Infrastructure;

namespace CourseProject.ConsoleForTests
{
    class Program
    {
        static void printMatrix(int[][] arr)
        {
            int N = 10;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    Console.Write(arr[i][j] + " ");
                Console.Write("\n");
            }
        }

        static void Main(string[] args)
        {
            IRandomService service = new FetchRandomService();

            Console.WriteLine(service.Next(3));
        }
    }
}
