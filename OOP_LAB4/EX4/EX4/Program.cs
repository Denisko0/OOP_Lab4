using System;
using System.Threading;

class Program
{
    static void Main()
    {
        int[,] matrixA = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
        int[,] matrixB = { { 8, 7, 6 }, { 5, 4, 3 } };
        int[,] result = new int[matrixA.GetLength(0), matrixB.GetLength(1)];

        
        int threadCount = 4;

        
        Semaphore semaphore = new Semaphore(threadCount, threadCount);

        
        object monitor = new object();

        
        for (int i = 0; i < threadCount; i++)
        {
            int threadNumber = i;
            new Thread(() =>
            {
                for (int j = threadNumber; j < matrixA.GetLength(0); j += threadCount)
                {
                    for (int k = 0; k < matrixB.GetLength(1); k++)
                    {
                        int sum = 0;
                        for (int l = 0; l < matrixB.GetLength(0); l++)
                        {
                            sum += matrixA[j, l] * matrixB[l, k];
                        }

                        
                        semaphore.WaitOne();
                        lock (monitor)
                        {
                            result[j, k] = sum;
                        }
                        semaphore.Release();
                    }
                }
            }).Start();
        }

        
        while (Thread.CurrentThread.GetManagedThreadId() != 1)
        {
            Thread.Sleep(100);
        }

        
        for (int i = 0; i < result.GetLength(0); i++)
        {
            for (int j = 0; j < result.GetLength(1); j++)
            {
                Console.Write(result[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}