using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCancellationTokenExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ExpensiveOperations expensiveOperations = new ExpensiveOperations();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task a15SecOperation = Task.Run(() => expensiveOperations.WaitFor15Seconds(cancellationTokenSource), cancellationTokenSource.Token);
            Task a20SecOperation = Task.Run(() => expensiveOperations.WaitFor20Seconds(cancellationTokenSource), cancellationTokenSource.Token);
            Task a25SecOperation = Task.Run(() => expensiveOperations.WaitFor25Seconds(cancellationTokenSource), cancellationTokenSource.Token);

            //cancellationTokenSource.Cancel();

            var tasks =Task.WhenAll(a15SecOperation, a20SecOperation, a25SecOperation).ContinueWith((a15SecO) =>
            {
                Console.WriteLine($"15 sec operation is Cancelled : {a15SecOperation.IsCanceled}");
                Console.WriteLine($"20 sec operation is Cancelled : {a20SecOperation.IsCanceled}");
                Console.WriteLine($"25 sec operation is Cancelled : {a25SecOperation.IsCanceled}");
            });

            tasks.Wait();

            Console.Read();
        }
    }

    class ExpensiveOperations
    {
        public void WaitFor15Seconds(CancellationTokenSource token)
        {
            for(int i = 0; i <= 15; i++)
            {
                Console.WriteLine($"15 Second Task Spanned For {i} seconds");               
                Thread.Sleep(1000);                
            }
            token.Cancel(); // This statement is used to trigger the cancel alert, so that other tasks can listen and stop based on their preference.
        }

        public void WaitFor20Seconds(CancellationTokenSource token)
        {
            for (int i = 0; i <= 20; i++)
            {
                Console.WriteLine($"20 Second Task Spanned For {i} seconds");
                // Even though cancel is called in WaitFor15Seconds program. It is upto this module to decide whether to stop the thread or not.  
                // When we used the below method it listens for cancel method if found, then it will throw a Cancellation Exception.
                token.Token.ThrowIfCancellationRequested(); 
                Thread.Sleep(1000);
            }
        }

        public void WaitFor25Seconds(CancellationTokenSource token)
        {
            for (int i = 0; i <= 20; i++)
            {
                Console.WriteLine($"20 Second Task Spanned For {i} seconds");
                // This method doesnt initiated ThrowIfCancellationRequested method So this task will not get cancelled.
                Thread.Sleep(1000);
            }
        }
    }
}
