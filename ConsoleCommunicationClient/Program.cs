using HslCommunication;
using HslCommunication.Enthernet;
using System;

/// <summary>
/// 通讯类 客户端发送信息
/// </summary>
namespace ConsoleCommunicationClient
{
    class Program
    {
        static int port = 17432;

        // 用于访问服务器数据的网络对象类，必须修改这个端口参数，否则运行失败
        public static NetSimplifyClient Net_simplify_client { get; set; } = new NetSimplifyClient("127.0.0.1", port: port)  // 指定服务器的ip，和服务器设置的端口
        {
            Token = Guid.Empty, // 这个guid码必须要服务器的一致，否则服务器会拒绝连接
            ConnectTimeOut = 5000,// 连接的超时时间
        };

        static void Main(string[] args)
        {
            Console.WriteLine("客户端启动！");

            OperateResult<string> result = null;

            while (true)
            {
                result = Net_simplify_client.ReadFromServer(new NetHandle(1, 0, 1), Console.ReadLine()); // 指示了大类1，子类0，编号1

                if (result.IsSuccess)
                {
                    // 按照上面服务器的代码，此处显示数据为："上传成功！返回的数据：测试数据大类1，命令1，接收到的数据是：发送的数据"
                    Console.WriteLine(result.Content);
                }
                else
                {
                    Console.WriteLine("操作失败！原因：" + result.Message);// 失败的原因基本上是连接不上，如果GUID码填写错误，也会连接不上
                }
            }

        }

    }
}
