using System;
using System.Threading.Tasks;
using OktaOAuthPOC.Client;

namespace OktaOAuthPOC
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                var client = new OktaClient();
                var token = Task.Run(async () => await client.GetBearerToken().ConfigureAwait(false)).Result;
                Console.WriteLine(token);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}