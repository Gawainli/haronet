using System.Net;
using System.Net.Sockets;
using System.Text;

namespace haronet.ProtobufServer;

public class ProtobufServer
{
    private static readonly List<TcpClient> Clients = new List<TcpClient>();
    
    public static Task Run()
    {
        foreach (var c in Clients)
        {
            c.Update();
        }
    }
    
    public static TcpClient CreateClient(INetPackageEncoder encoder, INetPackageDecoder decoder)
    {
        var client = new TcpClient(encoder, decoder);
        Clients.Add(client);
        return client;
    }
    
    public static void DestroyClient(TcpClient client)
    {
        client.Dispose();
        Clients.Remove(client);
    }
    
    public static async Task Update()
    {
        await Task.Delay(33);
        foreach (var c in Clients)
        {
            c.Update();
        }
    }
}