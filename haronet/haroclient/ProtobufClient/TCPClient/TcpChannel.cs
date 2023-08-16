using System.Net.Sockets;

namespace haronet.ProtobufServer;

public class TcpChannel : IDisposable
{
    public bool IsConnected => socket.Connected;
    
    private readonly Queue<INetPackage> sendQueue = new Queue<INetPackage>();
    private readonly Queue<INetPackage> recvQueue = new Queue<INetPackage>();
    private readonly List<INetPackage> decodeTempList = new List<INetPackage>();

    private byte[] recvBuffer = new byte[DefaultNetPackage.PkgMaxSize];
    private RingBuffer encodeBuffer = new RingBuffer(DefaultNetPackage.PkgMaxSize * 4);
    private RingBuffer decodeBuffer = new RingBuffer(DefaultNetPackage.PkgMaxSize * 4);
    
    private INetPackageDecoder decoder;
    private INetPackageEncoder encoder;
    private Socket socket;

    
    public TcpChannel(INetPackageDecoder decoder, INetPackageEncoder encoder, Socket socket)
    {
        this.decoder = decoder;
        this.encoder = encoder;
        this.socket = socket;
    }
    
    public void Update()
    {
        if (!socket.Connected)
        {
            return;
        }
        
        SendLoop();
        RecvLoop();
    }
    
    
    public void SendPkg(INetPackage pkg)
    {
        sendQueue.Enqueue(pkg);
    }
    
    
    public INetPackage RecvPkg()
    {
        if (recvQueue.Count == 0)
        {
            return null;
        }
        return recvQueue.Dequeue();
    }
    
    private async void SendLoop()
    {
        while (sendQueue.Count>0)
        {
            if (encodeBuffer.WriteableBytes < DefaultNetPackage.PkgMaxSize)
            {
                break;
            }
            var pkg = sendQueue.Dequeue();
            encoder.Encode(encodeBuffer, pkg);
        }
        var sendBytes = encodeBuffer.ReadBytes(encodeBuffer.ReadableBytes);
        await socket.SendAsync(sendBytes, SocketFlags.None);
    }
    
    private async void RecvLoop()
    {
        var recvBytes = await socket.ReceiveAsync(recvBuffer, SocketFlags.None);
        if (recvBytes == 0)
        {
            return;
        }

        if (!decodeBuffer.IsWriteable(recvBytes))
        {
            return;
        }
        
        decodeBuffer.WriteBytes(recvBuffer, 0, recvBytes);
        decodeTempList.Clear();
        decoder.Decode(decodeBuffer, decodeTempList);
        
        foreach (var pkg in decodeTempList)
        {
            recvQueue.Enqueue(pkg);
        }
        decodeTempList.Clear();
        RecvLoop();
    }

    public void Dispose()
    {
        try
        {
            socket.Shutdown(SocketShutdown.Both);

            sendQueue.Clear();
            recvQueue.Clear();
            decodeTempList.Clear();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            socket.Close();
        }
    }
    
}