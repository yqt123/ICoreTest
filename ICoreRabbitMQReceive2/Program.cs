/*
 * install-package rabbitmq.client
 */
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ICoreRabbitMQReceive2
{
    class Program
    {
        const string HOSTNAME = "47.112.114.44";
        const int PORT = 5672;
        const string USERNAME = "remote_guest";
        const string PASSWORD = "guest";

        static void Main(string[] args)
        {
            //一对一或一对多模式，一对多时 按顺序接收消息
            OneWorkerReceived(args);
            //手动确认消息，防止宕机消息没处理
            //OneWorkerReceived(args, autoAck: false);
            //是先能者多劳,哪个机子速度快就多跑一点
            //OneWorkerReceived(args, autoAck: false, basicQos: false);
            //订阅模式
            //ExchangeReceived(args, autoAck: false, basicQos: false, exchangetype: "direct", exchangeName: "exchange2", routingKey: "routingKey1");
        }

        /// <summary>
        /// 只启用一个消费者时为一对一模式
        /// 启用多个消费者时一对多模式
        /// </summary>
        /// <param name="args"></param>
        /// <param name="autoAck">消息自动确认</param>
        /// <param name="basicQos">告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息(basicQos = false时)</param>
        static void OneWorkerReceived(string[] args, bool autoAck = true, bool basicQos = true)
        {
            Console.WriteLine("ICoreRabbitMQReceive2一对一模式！");
            IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = HOSTNAME,//IP地址
                Port = PORT,//端口号
                UserName = USERNAME,//用户账号
                Password = PASSWORD//用户密码
            };
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
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

                    //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                    channel.BasicQos(0, 1, basicQos);

                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] message = ea.Body;//接收到的消息
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));

                        Thread.Sleep((new Random().Next(1, 6)) * 1000);//随机等待,实现能者多劳,

                        if (!autoAck)
                            //返回消息确认
                            channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听
                    channel.BasicConsume(queue: queueName, autoAck: autoAck, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// 订阅模式
        /// </summary>
        /// <param name="args"></param>
        /// <param name="autoAck">消息自动确认</param>
        /// <param name="basicQos">告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息(basicQos = false时)</param>
        static void ExchangeReceived(string[] args, bool autoAck = true, bool basicQos = true, string exchangetype = "fanout", string exchangeName = "exchange1", string routingKey = "")
        {
            //创建一个随机数,以创建不同的消息队列
            int random = new Random().Next(1, 1000);
            Console.WriteLine("ExchangeReceived Start" + random.ToString());

            IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = HOSTNAME,//IP地址
                Port = PORT,//端口号
                UserName = USERNAME,//用户账号
                Password = PASSWORD//用户密码
            };
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: exchangetype);

                    //消息队列名称
                    String queueName = exchangeName + "_" + random.ToString();

                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //将队列与交换机进行绑定
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

                    //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                    channel.BasicQos(0, 1, basicQos);

                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] message = ea.Body;//接收到的消息
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));

                        Thread.Sleep((new Random().Next(1, 6)) * 1000);//随机等待,实现能者多劳,

                        if (!autoAck)
                            //返回消息确认
                            channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听
                    channel.BasicConsume(queue: queueName, autoAck: autoAck, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }

    }
}
