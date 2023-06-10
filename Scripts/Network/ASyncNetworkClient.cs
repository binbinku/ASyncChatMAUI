using System.Net;
using System.Net.Sockets;
using System.Text;

public class ASyncNetworkClient
{

    #region 字段
    private static Socket clientSocket;

    private static string serverIP = "127.0.0.1";

    private static int serverPort = 14524;

    private static IPEndPoint serverIPEndPoint;

    private static byte[] recData;

    public static Action<string> recCallBackDelegate;//接收回调委托

    public static Action<bool> connectCallBackDelegate;//连接结果回调委托
    #endregion

    #region 初始化
    public static void Init()
    {

        Console.WriteLine("[ANC]:启动初始化程序");

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        serverIPEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

    }
    #endregion

    #region 异步连接

    public static void ASyncConnect()
    {
        Console.WriteLine("[ANC]:开始异步连接服务器");
        clientSocket.BeginConnect(serverIPEndPoint, ASyncConnectCallBack, clientSocket);
    }

    private static void ASyncConnectCallBack(IAsyncResult ar)
    {
        Console.WriteLine("[ANC]:异步连接回调");

        Socket cs = null;

        try
        {
            cs = ar.AsyncState as Socket;

            if (cs != null)
            {
                cs.EndConnect(ar);

                Console.WriteLine("[ANC]:已与服务器成功建立连接");

                //连接成功回调
                if (connectCallBackDelegate != null)
                    connectCallBackDelegate(true);

                ASyncReceive();
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine("[ANC]:连接服务器超时");

            if (connectCallBackDelegate != null)
                connectCallBackDelegate(false);

        }

    }

    #endregion

    #region 异步接收
    public static void ASyncReceive()
    {
        recData = new byte[1024];

        Console.WriteLine("[ANC]:开始异步接收消息");

        clientSocket.BeginReceive(recData, 0, 1024, 0, ASyncReceiveCallBack, clientSocket);

    }

    public static void ASyncReceiveCallBack(IAsyncResult ar)
    {
        Console.WriteLine("[ANC]:接收回调");
        int recLength = -1;
        try
        {
            recLength = clientSocket.EndReceive(ar);

            string recStr = Encoding.UTF8.GetString(recData);

            Console.WriteLine(recStr);

            //接收到消息,调用委托进行消息分发
            if (recCallBackDelegate != null)
                recCallBackDelegate(recStr);

            clientSocket.BeginReceive(recData, 0, 1024, 0, ASyncReceiveCallBack, clientSocket);

        }
        catch (SocketException ex)
        {
            Console.WriteLine("[ANC]:异步接收异常" + ex.ToString());
        }

    }

    #endregion

    #region 同步发送
    public static void SyncSend(string data)
    {
        byte[] sendData = Encoding.UTF8.GetBytes(data);

        clientSocket.Send(sendData);
    }
    #endregion


    #region 同步断开链接

    public static void SyncDisconnect()
    {
        clientSocket.Disconnect(false);
    }

    #endregion

}