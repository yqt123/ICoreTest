using ServiceStack;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICoreRedis
{
    class RedisManage
    {
        public static PooledRedisClientManager ClientManager { get; private set; }
        static RedisManage()
        {
            RedisClientManagerConfig redisConfig = new RedisClientManagerConfig();
            redisConfig.MaxWritePoolSize = 128;
            redisConfig.MaxReadPoolSize = 128;

            //可以读写分离，指定一台服务器读，一台写。
            // new PooledRedisClientManage(读写的服务器地址，只读的服务器地址
            ClientManager = new PooledRedisClientManager(
                new string[] { "redis123@47.112.114.44:6379" },
                new string[] { "redis123@47.112.114.44:6379" }, redisConfig);
        }

        public static bool Set<T>(string key, T val)
        {
            using (IRedisClient con = RedisManage.ClientManager.GetClient())
            {
                return con.Set<T>(key, val);
            }
        }

        public static T Get<T>(string key)
        {
            using (IRedisClient con = RedisManage.ClientManager.GetClient())
            {
                return con.Get<T>(key);
            }
        }

        public static void AddItemToList(string key, string val)
        {
            using (IRedisClient con = RedisManage.ClientManager.GetClient())
            {
                con.AddItemToList(key, val);
            }
        }
    }
}
