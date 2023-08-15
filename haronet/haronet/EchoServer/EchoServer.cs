using System.Net;
using System.Net.Sockets;
using System.Text;

namespace haronet.EchoServer
{
    public class EchoServer
    {
        public static async Task Run()
        {
            Console.WriteLine("Run Echo Server!");
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
                try
                {
                    HandleClient(handler);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private static async void HandleClient(Socket client)
        {
            Console.WriteLine($"HandleClient: {client.RemoteEndPoint}");
            var buffer = new byte[1_024];
            while (true)
            {
                // Receive message.
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);

                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("Client disconnected.");
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    return;
                }

                // var eom = "<|EOM|>";
                // if (response.IndexOf(eom) > -1 /* is end of message */)
                {
                    Console.WriteLine(
                        $"Socket server received message: \"{response}\"");

                    var ackMessage = "<|ACK|>";
                    var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                    await client.SendAsync(echoBytes, 0);
                    Console.WriteLine(
                        $"Socket server sent acknowledgment: \"{ackMessage}\"");
                    // break;
                }
            }
        }

        public static void PrintEcho(string message)
        {
            Console.WriteLine(message);
        }
    }
}