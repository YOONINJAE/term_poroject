using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

enum csConnStatus //소켓의 상태
{
    Closed,
    Listening,
    Connecting,
    Connected
};

static class TSocket
{
    public static char sSTX() { return Convert.ToChar(0x02); }
    public static char sETX() { return Convert.ToChar(0x03); }
    public static char sEOT() { return Convert.ToChar(0x04); }
    public static char sENQ() { return Convert.ToChar(0x05); }
    public static char sACK() { return Convert.ToChar(0x06); }
    public static char sNAK() { return Convert.ToChar(0x15); }
    public static char sCR() { return Convert.ToChar(13); }
    public static char sLF() { return Convert.ToChar(10); }
    public static string sCRLF() { return "\r\n"; }

    public static string HostName()
    {
        return Dns.GetHostName();
    }

    public static IPAddress[] HostAddresses()
    {
        string hostname = HostName();
        IPAddress[] ips = Dns.GetHostAddresses(hostname);
        return ips;
    }
}

// 데이터 수신 이벤트핸들링 함수 delegate 원형
public delegate void ServerDataArrivalHandler();

class TServer
{
    private const int buffersize = 1024;

    // Server용 Socket 객체
    private TcpListener listener = null;        // Listening중에만존재, 연결되면null
    private TcpClient clientForServer = null;
    private NetworkStream streamServer = null;

    // 현재의 상태
    private csConnStatus serverStatus = csConnStatus.Closed;

    // Server용 수신 thread
    private Thread threadServerRcv = null;
    private Thread threadChkPartnerDeath = null;

    // Server 수신 데이터
    private string serverRcvMessage = "";

    // 수신이벤트를 위한 델리게이트
    private ServerDataArrivalHandler DataArrivalCallback;

    //===============================================================
    //  생성자 1 : 수신데이터 이벤트핸들링 없은 경우.
    //  생성자 2 : 수신데이터 이벤트핸들링 함수를 지정하는 경우
    //===============================================================
    public TServer()
    {
        DataArrivalCallback = null;
    }
    public TServer(ServerDataArrivalHandler callback)
    {
        DataArrivalCallback = new ServerDataArrivalHandler(callback);
    }

    //===============================================================
    //  Server : Start
    //===============================================================
    public void ServerStartListen(string serverIP, int serverPort)
    {
        if (serverStatus == csConnStatus.Listening) return;
        if (serverStatus == csConnStatus.Connected) return;

        // 1단계 : Start
        try
        {
            // server측 접속 IP 객체 얻기
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

            // server 객체 얻기
            listener = new TcpListener(serverAddress);

            // Start
            listener.Start();
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
            return;
        }

        // 2단계 : Listen 시작
        bool success = ServerBeginListen();

        serverStatus = csConnStatus.Listening;
    }

    //===============================================================
    //  Server : Listen 시작 [Async 모드]
    //===============================================================
    private bool ServerBeginListen()
    {
        try  // listening중에 다시 같은 포트로 들어오는 경우 대비
        {
            listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), listener);
        }
        catch { return false; }

        return true;
    }

    //---------------------------------------------------------------
    //  Listen CallBack 함수
    //---------------------------------------------------------------
    private void DoAcceptTcpClientCallback(IAsyncResult ar)
    {
        try  // Listen 대기중 Close 했을때의 exception 처리를 위해
        {
            // Get the listener that handles the client request.
            listener = (TcpListener)ar.AsyncState;

            // End the operation
            clientForServer = listener.EndAcceptTcpClient(ar);

            // stream 객체 얻기
            streamServer = clientForServer.GetStream();

            // 수신 thread 시작
            threadServerRcv = new Thread(ServerReceiveThreadMain);
            threadServerRcv.IsBackground = true;
            threadServerRcv.Start();

            // 상대 돌발죽음 감시 thread 시작
            threadChkPartnerDeath = new Thread(ServerCheckPartnerDeathThread);
            threadChkPartnerDeath.IsBackground = true;
            threadChkPartnerDeath.Start();

            serverStatus = csConnStatus.Connected;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            serverStatus = csConnStatus.Closed;
        }
        finally
        {
            // 연결되건, 최소되건 listener는 필요 없어짐
            listener.Stop();
            listener = null;
        }
    }

    //===============================================================
    //  Server : Close
    //===============================================================
    public void ServerClose()
    {
        if (clientForServer != null) clientForServer.Close();
        if (listener != null) listener.Stop();

        while (true)
        {   // listening중에 close되면 
            // DoAcceptTcpClientCallback()에서 listener처리할때까지 대기
            if (listener == null) break;
        }

        // 순서 바뀌면 안됨
        if (threadServerRcv != null && threadServerRcv.IsAlive)
        {
            threadServerRcv.Abort();
            threadServerRcv.Join();
        }

        if (threadChkPartnerDeath != null && threadChkPartnerDeath.IsAlive)
        {
            threadChkPartnerDeath.Abort();
            threadChkPartnerDeath.Join();
        }

        serverStatus = csConnStatus.Closed;
    }

    //===============================================================
    //  Server Status
    //===============================================================
    public csConnStatus ServerStatus()
    {
        return serverStatus;
    }

    //===============================================================
    //  Server Send
    //===============================================================
    public void ServerSend(string st)
    {
        try
        {
            if (streamServer == null || !streamServer.CanWrite) return;
            byte[] msg = Encoding.UTF8.GetBytes(st);
            streamServer.Write(msg, 0, msg.Length);
        }
        catch { }
    }

    //===============================================================
    //  수신 데이터 꺼내기
    //===============================================================
    public string GetRcvMsg()
    {
        lock (serverRcvMessage)  //<- thread간 동기화
        {
            string tmp = serverRcvMessage;
            serverRcvMessage = "";
            return tmp;
        }
    }

    //===============================================================
    //  Server Receive Thread Main
    //===============================================================
    private void ServerReceiveThreadMain()
    {
        try
        {
            byte[] bytebuff = new byte[buffersize];
            while (true)
            {
                Thread.Sleep(1);

                if (streamServer == null) continue;
                if (!streamServer.CanRead) continue;

                StringBuilder strbuff = new StringBuilder();
                int nbyteRead = 0;

                // 도착 message가 buffer 크기보다 큰 경우를 위해 loop
                do
                {
                    nbyteRead = streamServer.Read(bytebuff, 0, bytebuff.Length);    // blocking
                    if (nbyteRead == 0)
                    {   // 상대방이 close했음을 감지
                        serverStatus = csConnStatus.Closed;
                        ServerClose();              //<- 없으면 안됨!!
                    }
                    strbuff.AppendFormat("{0}", Encoding.UTF8.GetString(bytebuff, 0, nbyteRead));
                }
                while (streamServer.DataAvailable);

                // 수신버퍼에 복사
                lock (serverRcvMessage)  //<- thread간 동기화
                {
                    serverRcvMessage += strbuff;
                }

                // 데이터 수신 callback 함수 호출
                if (DataArrivalCallback != null) { DataArrivalCallback(); }
            }
        }
        catch { }
    }

    //===============================================================
    //  Server Check Partner Death Thread
    //===============================================================
    private void ServerCheckPartnerDeathThread()
    {
        try
        {
            while (true)
            {
                Thread.Sleep(5);

                // 상대방 client가 connected에서 갑자기 꺼져 버리면
                // Closed로 바꾸는 방법이 정기적인 검사밖에 없음. 
                // 상황보다 약간 늦게 알게되지만 상관없음.
                if (serverStatus == csConnStatus.Connected)
                {
                    if (clientForServer != null && clientForServer.Client != null)
                    {
                        if (clientForServer.Client.Connected == false)
                            serverStatus = csConnStatus.Closed;
                    }
                }
                if (serverStatus != csConnStatus.Connected) break;
            }
        }
        catch { }
    }
}
// 데이터 수신 이벤트핸들링 함수 delegate 원형
public delegate void ClientDataArrivalHandler();

class TClient
{
    private const int buffersize = 1024;

    // Client용 Socket 객체
    private TcpClient clientForClient = null;
    private NetworkStream streamClient = null;

    // 현재의 상태
    private csConnStatus clientStatus = csConnStatus.Closed;

    // Client용 수신 thread
    private Thread threadClientRcv = null;
    private Thread threadChkPartnerDeath = null;

    // Client 수신 데이터
    private string clientRcvMessage = "";

    // 수신이벤트를 위한 델리게이트
    private ClientDataArrivalHandler DataArrivalCallback;
    //===============================================================
    //  생성자 1 : 수신데이터 이벤트핸들링 없은 경우.
    //  생성자 2 : 수신데이터 이벤트핸들링 함수를 지정하는 경우
    //===============================================================
    public TClient()
    {
        DataArrivalCallback = null;
    }
    public TClient(ClientDataArrivalHandler callback)
    {
        DataArrivalCallback = new ClientDataArrivalHandler(callback);
    }

    //===============================================================
    //  Client : Connect 시도 [Async 모드]
    //===============================================================
    public void ClientBeginConnect(string serverIP, int serverPort, string clientIP)
    {
        if (clientStatus == csConnStatus.Connected) return;
        if (clientStatus == csConnStatus.Connecting) return;

        // server측 접속 IP 객체 얻기
        IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

        // client측 접속 IP 객체 얻기
        // port = 0은 임의의 할당포트 사용하겠다는 뜻
        int dumport = 0;
        IPEndPoint clientAddress = new IPEndPoint(IPAddress.Parse(clientIP), dumport);

        // client 객체 얻기
        clientForClient = new TcpClient(clientAddress);

        // connected 시작
        IPAddress serverIpAddress = IPAddress.Parse(serverIP);
        clientForClient.BeginConnect(serverIpAddress, serverPort, new AsyncCallback(DoConnectTcpClientCallback), clientForClient);

        clientStatus = csConnStatus.Connecting;
    }

    //---------------------------------------------------------------
    //  Connect CallBack 함수
    //---------------------------------------------------------------
    private void DoConnectTcpClientCallback(IAsyncResult ar)
    {
        try
        {
            clientForClient = (TcpClient)ar.AsyncState;
            clientForClient.EndConnect(ar);
            streamClient = clientForClient.GetStream(); // stream 객체 얻기

            // 수신 thread 시작
            threadClientRcv = new Thread(ClientReceiveThreadMain);
            threadClientRcv.IsBackground = true;
            threadClientRcv.Start();

            // 상대 돌발죽음 감시 thread 시작
            threadChkPartnerDeath = new Thread(ClientCheckPartnerDeathThread);
            threadChkPartnerDeath.IsBackground = true;
            threadChkPartnerDeath.Start();

            clientStatus = csConnStatus.Connected;
        }
        catch
        {
            clientStatus = csConnStatus.Closed;
        }
    }

    //===============================================================
    //  Client : Close
    //===============================================================
    public void ClientClose()
    {
        if (clientForClient != null) clientForClient.Close();
        clientForClient = null;

        // 순서 바뀌면 안됨
        if (threadClientRcv != null && threadClientRcv.IsAlive)
        {
            threadClientRcv.Abort();
            threadClientRcv.Join();
        }

        if (threadChkPartnerDeath != null && threadChkPartnerDeath.IsAlive)
        {
            threadChkPartnerDeath.Abort();
            threadChkPartnerDeath.Join();
        }

        clientStatus = csConnStatus.Closed;
    }

    //===============================================================
    //  Client Status
    //===============================================================
    public csConnStatus ClientStatus()
    {
        return clientStatus;
    }//클라이언트 상태 체크 물어보는거

    //===============================================================
    //  Client Send
    //===============================================================
    public void ClientSend(string st)
    {
        try
        {
            if (streamClient == null || !streamClient.CanWrite) return;
            byte[] msg = Encoding.UTF8.GetBytes(st);
            streamClient.Write(msg, 0, msg.Length);
        }
        catch { }
    }//보내는거

    //===============================================================
    //  수신 데이터 꺼내기
    //===============================================================
    public string GetRcvMsg()
    {
        lock (clientRcvMessage)  //<- thread간 동기화
        {
            string tmp = clientRcvMessage;
            clientRcvMessage = "";
            return tmp;
        }
    }//버퍼에서 데이터 읽는거

    //===============================================================
    //  Client Receive Thread Main
    //===============================================================
    private void ClientReceiveThreadMain()
    {
        try
        {
            byte[] bytebuff = new byte[buffersize];
            while (true)
            {
                Thread.Sleep(1);

                if (streamClient == null) continue;
                if (!streamClient.CanRead) continue;

                StringBuilder strbuff = new StringBuilder();
                int nbyteRead = 0;

                // 도착 message가 buffer 크기보다 큰 경우를 위해 loop
                do
                {
                    nbyteRead = streamClient.Read(bytebuff, 0, bytebuff.Length);    // blocking
                    if (nbyteRead == 0)
                    {   // 상대방이 close했음을 감지
                        clientStatus = csConnStatus.Closed;
                        ClientClose();
                    }
                    strbuff.AppendFormat("{0}", Encoding.UTF8.GetString(bytebuff, 0, nbyteRead));
                }
                while (streamClient.DataAvailable);

                // 수신버퍼에 복사
                lock (clientRcvMessage)  //<- thread간 동기화
                {
                    clientRcvMessage += strbuff;
                }

                // 데이터 수신 callback 함수 호출
                if (DataArrivalCallback != null) { DataArrivalCallback(); }
            }
        }
        catch
        { }
    }

    //===============================================================
    //  Client Check Partner Death Thread
    //===============================================================
    private void ClientCheckPartnerDeathThread()
    {
        try
        {
            while (true)
            {
                Thread.Sleep(5);

                // 상대방이 connected에서 갑자기 꺼져 버리면
                // Closed로 바꾸는 방법이 정기적인 검사밖에 없음. 
                // 상황보다 약간 늦게 알게되지만 상관없음.
                if (clientStatus == csConnStatus.Connected)
                {
                    if (clientForClient != null && clientForClient.Client != null)
                    {
                        if (clientForClient.Client.Connected == false)
                            clientStatus = csConnStatus.Closed;
                    }
                }
                if (clientStatus != csConnStatus.Connected) break;
            }
        }
        catch { }
    }
}