using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/*
from 临时变量 in 实现IEnumerable<T>接口的对象
where条件表达式
[orderby 条件]
[group by 条件]
select 临时变量中被查询的值   
*/
namespace ConsoleLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IEnumerable<string> list = new List<string>() { "a", "b", "c" };

            var d = from n in list where n == "a" orderby n select n;

            foreach (var item in d) {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
