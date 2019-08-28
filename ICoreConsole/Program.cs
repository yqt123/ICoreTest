/*
 * install-package rabbitmq.client
 */
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace ICoreRabbitMQSend
{
    class Program
    {
        const string HOSTNAME = "47.112.114.44";
        const int PORT = 5672;
        const string USERNAME = "remote_guest";
        const string PASSWORD = "guest";

        static void Main(string[] args)
        {
            WorkerSend(args);
            //订阅模式
            //ExchangeSend(args, "direct", "exchange2", "routingKey2");
        }

        /// <summary>
        /// 一对一模式，和一对多模式通用
        /// </summary>
        /// <param name="args"></param>
        static void WorkerSend(string[] args)
        {
            Console.WriteLine("WorkerSend！");
            IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = HOSTNAME,//IP地址
                Port = PORT,//端口号
                UserName = USERNAME,//用户账号
                Password = PASSWORD//用户密码
            };
            using (IConnection con = conFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    String queueName = String.Empty;
                    if (args.Length > 0)
                        queueName = args[0];
                    else
                        queueName = "queue1";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }

        /// <summary>
        /// 订阅模式
        /// </summary>
        /// <param name="args"></param>
        /// <param name="exchangetype">fanout </param>
        static void ExchangeSend(string[] args, string exchangetype, string exchangeName = "exchange1", string routingKey = "")
        {
            Console.WriteLine("ExchangeSend！");
            IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = HOSTNAME,//IP地址
                Port = PORT,//端口号
                UserName = USERNAME,//用户账号
                Password = PASSWORD//用户密码
            };
            using (IConnection con = conFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: exchangetype);
                    while (true)
                    {
                        Console.WriteLine("消息内容:");
                        String message = Console.ReadLine();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: exchangeName, routingKey: "routingKey1", basicProperties: null, body: body);
                        channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                }
            }
        }
    }
}
