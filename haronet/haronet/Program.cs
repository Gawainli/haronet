// See https://aka.ms/new-console-template for more information

using haronet.ProtobufServer;

Console.WriteLine("Run Server!");
await Task.Run(ProtobufServer.Run);
var delay = 33;

while (true)
{
    await Task.Delay(delay);
    ProtobufServer.Update();
}
