using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerConcurrent
{
    class Program
    {
        async Task RunServer()
        {
            var listener = new TcpListener(IPAddress.Any, 1234);
            
            try
            {
                listener.Start();
                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    await HandleClient(client);
                }
            }
            finally
            {
                listener.Stop();
            }
        }

        async Task HandleClient(TcpClient client)
        {
            // Instructs asynchronous method to give up CPU time for time being
            await Task.Yield();
            
            // Below will execute in the background
            using (client)
            {
                using (var stream = client.GetStream())
                {
                    Console.WriteLine("A client connected!");
                    var writer = new BinaryWriter(stream);
                    var reader = new BinaryReader(stream);

                    var fromClient = reader.ReadString();
                    Console.WriteLine($"Received: {fromClient} from client");
                        
                    writer.Write("Who is there?");
                    writer.Flush();
                }
            }
        }
        
        static async Task Main(string[] args)
        {
            var serverController = new Controller();
            await serverController.Run();
            
            // var program = new Program();
            // await program.RunServer();
        }
    }
}