using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace ICoreRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisManage.Set<string>("testkey", "123");

            var val = RedisManage.Get<string>("testkey");

            Console.WriteLine(val);
        }
    }
}
