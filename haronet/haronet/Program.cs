// See https://aka.ms/new-console-template for more information

using haronet.EchoServer;
using haronet.ProtobufServer;

Console.WriteLine("Run Server!");
await ProtobufServer.Run();