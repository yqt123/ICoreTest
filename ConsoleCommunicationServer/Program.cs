using System;
using System.Net;
using HslCommunication;
using HslCommunication.BasicFramework;
using HslCommunication.Enthernet;
using HslCommunication.LogNet;

/// <summary>
/// 通讯类 服务端接受信息
/// </summary>
namespace ConsoleCommunicationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("服务端启动!");
            Net_simplify_server server = new Net_simplify_server();
            server.Net_Simplify_Server_Initialization();
            Console.ReadLine();
        }
    }

    class Net_simplify_server
    {
        int port = 17432;

        // 用户同步数据传送的引擎
        private NetSimplifyServer net_simplify_server = new NetSimplifyServer(); //实例化
                                                                                 // 同步传送数据的初始化
        public void Net_Simplify_Server_Initialization()
        {
            try
            {
                net_simplify_server.Token = Guid.Empty;//设置身份令牌，本质就是一个GUID码，验证客户端使用
                net_simplify_server.LogNet = new LogNetSingle(@"E\simplify_log.txt");//日志路径，单文件存储模式，采用组件信息
                net_simplify_server.LogNet.SetMessageDegree(HslMessageDegree.DEBUG);//默认debug及以上级别日志均进行存储，根据需要自行选择，DEBUG存储的信息比较多
                net_simplify_server.ReceiveStringEvent += Net_simplify_server_ReceiveStringEvent;//接收到字符串触发
                net_simplify_server.ReceivedBytesEvent += Net_simplify_server_ReceivedBytesEvent;//接收到字节触发
                net_simplify_server.ServerStart(port);//网络端口，此处使用了一个随便填写的端口
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                var val = SoftBasic.GetExceptionMessage(ex);
                Console.WriteLine(val);
            }
        }

        /// <summary>
        /// 接收来自客户端的字节数据
        /// </summary>
        /// <param name="state">网络状态</param>
        /// <param name="customer">字节数据，根据实际情况选择是否使用</param>
        /// <param name="data">来自客户端的字节数据</param>
        private void Net_simplify_server_ReceivedBytesEvent(HslCommunication.Core.Net.AppSession state, NetHandle customer, byte[] data)
        {
            if (customer == 1000)
            {
                // 收到指令为1000的请求时，返回1000长度的字节数组
                net_simplify_server.SendMessage(state, customer, new byte[1000]);
            }
            else
            {
                net_simplify_server.SendMessage(state, customer, data);
            }
        }


        /***********************************************************************************************
         *
         *    方法说明：    当接收到来自客户端的数据的时候触发的方法
         *    特别注意：    如果你的数据处理中引发了异常，应用程序将会奔溃，SendMessage异常系统将会自动处理
         *
         ************************************************************************************************/

        /// <summary>
        /// 接收到来自客户端的字符串数据，然后将结果发送回客户端，注意：必须回发结果
        /// </summary>
        /// <param name="state">客户端的地址</param>
        /// <param name="handle">用于自定义的指令头，可不用，转而使用data来区分</param>
        /// <param name="data">接收到的服务器的数据</param>
        private void Net_simplify_server_ReceiveStringEvent(HslCommunication.Core.Net.AppSession state, NetHandle handle, string data)
        {

            /*******************************************************************************************
             *
             *     说明：同步消息处理总站，应该根据不同的消息设置分流到不同的处理方法
             *    
             *     注意：处理完成后必须调用 net_simplify_server.SendMessage(state, customer, "处理结果字符串，可以为空");
             *
             *******************************************************************************************/

            Console.WriteLine("接受到数据：" + data);

            if (handle.CodeMajor == 1)
            {
                ProcessCodeMajorOne(state, handle, data);
            }
            else if (handle.CodeMajor == 2)
            {
                ProcessCodeMajorTwo(state, handle, data);
            }
            else if (handle.CodeMajor == 3)
            {
                ProcessCodeMajorThree(state, handle, data);
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }

        private void ProcessCodeMajorOne(HslCommunication.Core.Net.AppSession state, NetHandle handle, string data)
        {
            if (handle.CodeIdentifier == 1)
            {
                // 下面可以再if..else
                net_simplify_server.SendMessage(state, handle, "测试数据大类1，命令1，接收到的数据是：" + data);
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }

        private void ProcessCodeMajorTwo(HslCommunication.Core.Net.AppSession state, NetHandle handle, string data)
        {
            if (handle.CodeIdentifier == 1)
            {
                // 下面可以再if..else
                net_simplify_server.SendMessage(state, handle, "测试数据大类2，命令1，接收到的数据是：" + data);
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }

        private void ProcessCodeMajorThree(HslCommunication.Core.Net.AppSession state, NetHandle handle, string data)
        {
            if (handle.CodeIdentifier == 1)
            {
                // 下面可以再if..else
                net_simplify_server.SendMessage(state, handle, "测试数据大类3，命令1，接收到的数据是：" + data);
            }
            else
            {
                net_simplify_server.SendMessage(state, handle, data);
            }
        }

    }
}
