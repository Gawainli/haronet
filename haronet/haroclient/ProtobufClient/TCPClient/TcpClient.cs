using System.Net.Sockets;

namespace haronet.ProtobufServer;

public class TcpClient : IDisposable
{
    private TcpChannel channel;
    private readonly INetPackageEncoder encoder;
    private readonly INetPackageDecoder decoder;

    public TcpClient(INetPackageEncoder encoder, INetPackageDecoder decoder)
    {
        this.encoder = encoder;
        this.decoder = decoder;
    }
    
    public void SendPkg(int msgId, byte[] bodyBytes)
    {
        var pkg = new DefaultNetPackage
        {
            MsgId = msgId,
            BodyBytes = bodyBytes
        };
        channel.SendPkg(pkg);
    }
    
    public void SendPkg(INetPackage pkg)
    {
        channel.SendPkg(pkg);
    }
    
    public INetPackage RecvPkg()
    {
        return channel.RecvPkg();
    }
    
    public bool Connected()
    {
        return channel.IsConnected;
    }
    
    public async Task Connect(string ip, int port)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        await socket.ConnectAsync(ip, port);
        channel = new TcpChannel(decoder, encoder, socket);
    }
    
    public void Update()
    {
        channel.Update();
    }

    public void Dispose()
    {
        channel.Dispose();
    }
}