using System.Threading.Tasks;

namespace Client
{
    class Program
    {
       static async Task Main(string[] args)
       {
           var quit = false;
           var client = new ClientController();
           
           while (!quit)
           {
               quit = await client.Run();
           }
       }
    }
}