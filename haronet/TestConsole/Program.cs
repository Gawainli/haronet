// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
Console.WriteLine("This is main thread. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
await Task.Run(AsyncTest);
// AsyncTest();
Console.WriteLine("This is main thread after AsyncTest(). ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
Console.WriteLine("All Done");
Console.ReadKey();


async void AsyncTest()
{
    Console.WriteLine("This is AsyncTest. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
    // var task = Task.Run(() =>
    // {
    //     Thread.Sleep(1000);
    //     Console.WriteLine("This is Task.Run. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
    //     Thread.Sleep(1000);
    //     Console.WriteLine("This is Task.Run. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
    // });
    // Console.WriteLine("This is AsyncTest before task.wait. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
    // await task;
    // Console.WriteLine("This is AsyncTest after task.wait. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
    for (int i = 0; i < 3; i++)
    {
        Console.WriteLine($"This is AsyncTest before Task.Delay. ThreadId: {Thread.CurrentThread.ManagedThreadId} i: {i}");
        await Task.Delay(TimeSpan.FromSeconds(1));
        Console.WriteLine("This is AsyncTest after Task.Delay. ThreadId: {0}", Thread.CurrentThread.ManagedThreadId);
    }
}