using System.Net;
using System.Net.Sockets;
using System.Text;

namespace haronet.ProtobufServer;

public class ProtobufServer
{
    private static readonly List<TcpChannel> Channels = new List<TcpChannel>();

    public static async Task Run()
    {
        Console.WriteLine("Run Protobuf Server!");
        var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11223);

        var listener = new Socket(
            ipEndPoint.AddressFamily,
            SocketType.Stream,
            ProtocolType.Tcp);
        listener.Bind(ipEndPoint);
        listener.Listen(100);

        while (true)
        {
            var handler = await listener.AcceptAsync();
            Console.WriteLine($"Socket server accepted client: {handler.RemoteEndPoint}");
            try
            {
                var channel = CreateChannel(new DefaultPkgDecoder(), new DefaultPkgEncoder(), handler);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            Update();
        }
    }

    public static TcpChannel CreateChannel(INetPackageDecoder decoder, INetPackageEncoder encoder, Socket socket)
    {
        var cl = new TcpChannel(decoder, encoder, socket);
        Channels.Add(cl);
        return cl;
    }

    public static void RemoveChannel(TcpChannel cl)
    {
        cl.Dispose();
        Channels.Remove(cl);
    }

    public static async void Update()
    {
        while (true)
        {
            await Task.Delay(1000/60);
            foreach (var c in Channels)
            {
                c.Update();
                if (c.RecvPkg() is DefaultNetPackage pkg)
                {
                    Console.WriteLine($"RecvPkg: {pkg.MsgId}");
                    var str = System.Text.Encoding.UTF8.GetString(pkg.BodyBytes);
                    Console.WriteLine($"RecvPkg msg: {str}");
                }
            }
        }
    }
}