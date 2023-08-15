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
            Socket handler = null;
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11223);

            var listener = new Socket(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);
                    listener.Bind(ipEndPoint);
                    listener.Listen(100);

            while (true)
            {
                try
                {
                    handler = await listener.AcceptAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine($" Accept exception: {e} ");
                    throw;
                }
                // Receive message.
                var buffer = new byte[1_024];
                var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);

                var eom = "<|EOM|>";
                if (response.IndexOf(eom) > -1 /* is end of message */)
                {
                    Console.WriteLine(
                        $"Socket server received message: \"{response.Replace(eom, "")}\"");

                    var ackMessage = "<|ACK|>";
                    var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                    await handler.SendAsync(echoBytes, 0);
                    Console.WriteLine(
                        $"Socket server sent acknowledgment: \"{ackMessage}\"");

                    break;
                }
                // Sample output:
                //    Socket server received message: "Hi friends 👋!"
                //    Socket server sent acknowledgment: "<|ACK|>"
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static void PrintEcho(string message)
        {
            Console.WriteLine(message);
        }
    }
}