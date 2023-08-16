using haronet.ProtobufServer;

namespace haroclient.ProtobufClient;

public class ProtobufClient
{
    public static TcpClient? Client;

    public static void Update()
    {
        Client?.Update();
        if (Client?.RecvPkg() is DefaultNetPackage pkg)
        {
            Console.WriteLine($"RecvPkg: {pkg.MsgId}");
        }
    }

    public static async Task CreateTcpClient()
    {
        var encoder = new DefaultPkgEncoder();
        var decoder = new DefaultPkgDecoder();
        var c = new TcpClient(encoder, decoder);
        await c.Connect("127.0.0.1", 11223);
        Client = c;
    }
}