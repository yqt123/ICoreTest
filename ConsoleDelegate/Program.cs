using System;

namespace ConsoleDelegate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //DelegateTest.MyDel myDel = DelegateTest.Add1;
            //myDel += DelegateTest.Add2;
            //DelegateTest.Calculate(myDel, 10);

            DelegateAction.Test2(DelegateTest.add2, 10, "dda");


            Console.ReadKey();
        }
    }

    class DelegateTest
    {

        public delegate int MyDel(int num);


        public static int Add1(int a)
        {
            int b = 10 + a;
            Console.WriteLine("——Add1———");
            return b;

        }

        public static int Add2(int a)
        {
            int b = 10 - a;
            Console.WriteLine("——Add2———");
            return b;

        }

        public static void Calculate(MyDel ex, int a)
        {
            var result = ex(a);
            Console.WriteLine(result + "\n");
        }

        public static void add(int a)
        {
            int b = 10 + a;
            Console.WriteLine($"——{b}———");
        }

        public static void add2(int a, string b)
        {
            int c = 10 + a;
            Console.WriteLine($"——{b},{c}———");
        }
    }

    class DelegateAction
    {
        public static void Test<T>(Action<T> action, T p)
        {
            action(p);
        }

        public static void Test2<T, T1>(Action<T, T1> action, T p, T1 p2)
        {
            action(p, p2);
        }

        public int Test3<T1>(Func<T1, int> func, T1 a)
        {
            return func(a);
        }

    }
}
