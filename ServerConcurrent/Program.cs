using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerConcurrent
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serverController = new Controller();
            await serverController.Run();
        }
    }
}