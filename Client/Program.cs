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
               await client.Run(QuitGame);
           }
       }

        private static void QuitGame(bool Quit)
        {
            return;
        }
    }
}