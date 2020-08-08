using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace ProgressExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ExpensiveOperation expensiveOperation = new ExpensiveOperation();
            Random random = new Random();
            List<int> numberList = new List<int>();
            Progress<double> progressInfo = new Progress<double>(ReportHandler);
            for (int i = 0; i < 127; i++)
            {
                numberList.Add(random.Next(1, 1000));
            }
            var task = Task.Run(() => expensiveOperation.GetEvenNumbersOperation(numberList, progressInfo));
            task.Wait();
            Console.Read();
        }

        static void ReportHandler(double value)
        {            
            Console.WriteLine($"Progressing ...{value.ToString("P1", CultureInfo.InvariantCulture)}"); // Obtains the Progress Information
        }
    }

    class ExpensiveOperation
    {
        public void GetEvenNumbersOperation(List<int> numberList, IProgress<double> progressInfo)
        {
            List<int> evenNumbers = new List<int>();
            int interval = numberList.Count / 10;
            for (int i = 0; i < numberList.Count; i++)
            {
                Thread.Sleep(500);
                if (numberList[i] % 2 == 0)
                {
                    evenNumbers.Add(numberList[i]);
                }
                if (interval != 0 && i % interval == 0)
                {
                    progressInfo.Report((double)i / numberList.Count); // Reports the Progress to the calling Function.
                }
            }
        }
    }
}
