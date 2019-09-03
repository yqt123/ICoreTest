using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTask
{
    class Program
    {
        static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ContinueWith();

            //Console.WriteLine("主线程执行业务处理.");
            //FunctionAsync();
            //Console.WriteLine("主线程执行其他处理");
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(string.Format("Main:i={0}", i));
            //}
            //Console.ReadLine();


        }

        static void ContinueWith()
        {
            stopwatch.Start();

            Task t = new Task(() =>
            {
                Console.WriteLine("任务开始工作……");
                //模拟工作过程
                Thread.Sleep(5000);
            });
            t.Start();
            t.ContinueWith((task) =>
            {
                Console.WriteLine("任务完成，完成时候的状态为：");
                Console.WriteLine("IsCanceled={0}\tIsCompleted={1}\tIsFaulted={2}", task.IsCanceled, task.IsCompleted, task.IsFaulted);
                Thread.Sleep(3000);
            });

            stopwatch.Stop();

            var d = stopwatch.ElapsedMilliseconds;
            Console.ReadKey();
        }

        static async void FunctionAsync()
        {
            await Task.Delay(1);
            Console.WriteLine("使用System.Threading.Tasks.Task执行异步操作.");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(string.Format("AsyncFunction:i={0}", i));
            }

        }


    }
}
