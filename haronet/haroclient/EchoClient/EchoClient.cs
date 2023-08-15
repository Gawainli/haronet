using System.Net;
using System.Net.Sockets;
using System.Text;

namespace haroclient.EchoClient;

public class EchoClient
{
    public static async Task Run()
    {
        Console.WriteLine("Run Echo Client!");
        var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11223);
        using Socket client = new(
            ipEndPoint.AddressFamily, 
            SocketType.Stream, 
            ProtocolType.Tcp);

        await client.ConnectAsync(ipEndPoint);
        while (true)
        {
            // Send message.
            // var message = "Hi friends 👋!<|EOM|>";
            var message = Console.ReadLine();
            if (string.IsNullOrEmpty(message) || message == "@exit")
            {
                Console.WriteLine("Exit!");
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                return;
            }
            
            var eom = "<|EOM|>";
            message += eom;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _ = await client.SendAsync(messageBytes, SocketFlags.None);
            Console.WriteLine($"Socket client sent message: \"{message}\"");

            // Receive ack.
            var buffer = new byte[1_024];
            var received = await client.ReceiveAsync(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            if (response == "<|ACK|>")
            {
                Console.WriteLine(
                    $"Socket client received acknowledgment: \"{response}\"");
                // break;
            }
            // Sample output:
            //     Socket client sent message: "Hi friends 👋!<|EOM|>"
            //     Socket client received acknowledgment: "<|ACK|>"
        }
    }
}