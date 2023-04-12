using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {

        int[] array = { 5, 3, 8, 6, 2, 7, 1, 4 };


        object lockObject = new object();


        Thread sortingThread = new Thread(() =>
        {
            QuickSort(array, 0, array.Length - 1, lockObject);
        });


        sortingThread.Start();


        sortingThread.Join();


        Console.WriteLine("Відсортований масив:");
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write(array[i] + " ");
        }
        Console.WriteLine();
    }

    static void QuickSort(int[] array, int left, int right, object lockObject)
    {
        if (left < right)
        {
            int pivotIndex = Partition(array, left, right, lockObject);


            Thread leftThread = new Thread(() =>
            {
                QuickSort(array, left, pivotIndex - 1, lockObject);
            });
            Thread rightThread = new Thread(() =>
            {
                QuickSort(array, pivotIndex + 1, right, lockObject);
            });


            leftThread.Start();
            rightThread.Start();


            leftThread.Join();
            rightThread.Join();
        }
    }

    static int Partition(int[] array, int left, int right, object lockObject)
    {

        int pivotIndex = (left + right) / 2;
        int pivotValue = array[pivotIndex];


        Swap(array, pivotIndex, right);


        int storeIndex = left;

        
        for (int i = left; i < right; i++)
        {
            if (array[i] < pivotValue)
            {
                Swap(array, i, storeIndex);
                
                storeIndex++;
            }
        }

        
        Swap(array, storeIndex, right);

        return storeIndex;
    }

    static void Swap(int[] array, int index1, int index2)
    {
        
        lock (lockObject)
        {
            int temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }
}