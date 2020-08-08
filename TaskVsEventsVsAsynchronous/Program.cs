using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace TaskVsEventsVsAsynchronous
{
    public delegate void MyHandler1(IProgress<int> progress);
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Parallel.ForEach(new List<int>() { 1, 2, 3, 4, 5 ,6 ,7, 8, 9, 10, 11, 12, 13, 14, 15}, async (i) =>
            {
                Progress<int> progress = new Progress<int>();
                progress.ProgressChanged += Progress_ProgressChanged;
                await new SampleTask().DoOperation(progress).ConfigureAwait(false);
                Console.WriteLine(i + " started");
            });
            Console.WriteLine("came here");

            //for (int i = 0; i <= 5; i++)
            //{
            //    await new SampleTask().DoOperation().ConfigureAwait(false);
            //    Console.wr("1 started");
            //}

            //new SampleTask().DoOperation();
            //new SampleTask().DoOperation();
            //new SampleTask().DoOperation();
            Console.ReadLine();
        }

        private static void Progress_ProgressChanged(object sender, int e)
        {
            Console.WriteLine("Completed " + e.ToString() + "%");
        }
    }

    class SampleTask
    {        
        public async Task DoOperation(Progress<int> progress)
        {
            var taskExpensiveOperation = Task.Run(() => new ExpensiveOperation().DoOperation(progress));
            Console.WriteLine("Async process initiaed");

            await taskExpensiveOperation.ConfigureAwait(false);
            Console.WriteLine("Completed.");
        }


    }

    class SampleEvents
    {
        public void DoOperation()
        {
            MyHandler1 d1 = new MyHandler1(new ExpensiveOperation().DoOperation);            
            IAsyncResult result = d1.BeginInvoke(new Progress<int>(), null, null);            
            Console.WriteLine("Async process initiaed");
            
            d1.EndInvoke(result);
            Console.WriteLine("Completed.");

        }
    }




    class SampleAsynchronous
    {

    }

    class ExpensiveOperation
    {
        public event MyHandler1 Event1;
        public void DoOperation(IProgress<int> progress)
        {
            for (int i = 0; i < 30; i++)
            {
                if (i == 15)
                {
                    progress.Report(50);                    
                }
                Thread.Sleep(1000);
            }            
        }
    }
}
