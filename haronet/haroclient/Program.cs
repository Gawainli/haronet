// See https://aka.ms/new-console-template for more information


using System.Text;
using haroclient.ProtobufClient;
using haronet.ProtobufServer;

Console.WriteLine("Run Client!");

await ProtobufClient.CreateTcpClient();
while (true)
{
    ProtobufClient.Update();
    await Task.Delay(33);

    var msg = Console.ReadLine();
    var pkg = new DefaultNetPackage()
    {
        MsgId = 1,
        BodyBytes = Encoding.UTF8.GetBytes(msg)
    };
    ProtobufClient.Client?.SendPkg(pkg);
}