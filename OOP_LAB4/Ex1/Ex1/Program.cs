using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static Queue<int> queue = new Queue<int>(); 
    static object lockObject = new object(); 

    static void Main(string[] args)
    {
        
        Thread producerThread = new Thread(Producer);
        producerThread.Start();

        
        Thread consumerThread = new Thread(Consumer);
        consumerThread.Start();

        
        producerThread.Join();
        consumerThread.Join();
    }

    static void Producer()
    {
        Random random = new Random();
        for (int i = 0; i < 10; i++)
        {
            int number = random.Next(100);
            lock (lockObject)
            {
                queue.Enqueue(number);
                Console.WriteLine($"Виробник додав число {number} до черги");
            }
            Thread.Sleep(500); 
        }
    }

    static void Consumer()
    {
        for (int i = 0; i < 10; i++)
        {
            int number;
            lock (lockObject)
            {
                if (queue.Count > 0)
                {
                    number = queue.Dequeue();
                    Console.WriteLine($"Споживач взяв число {number} з черги");
                }
                else
                {
                    Console.WriteLine("Споживач очікує на нові числа в черзі...");
                }
            }
            Thread.Sleep(1000);
        }
    }
}