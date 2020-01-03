using BankingApp.Api.Models;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.ConsoleClient
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();


        private static async Task MainAsync()
        {
            //Discover Endpoints using Metadata of IdentityServer 

            var Discovery = await DiscoveryClient.GetAsync("http://localhost:5000");

            if (Discovery.IsError)
            {
                Console.WriteLine(Discovery.Error);
            }



            
            //Grab a Bearer Token Using Client Credentials FLow Grant Type 

            var tokenClient = new TokenClient(Discovery.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("bankApi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }
            Console.WriteLine("\nGrant Type Flow\n\n");
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");


            //Grab a Bearer Token Using Resource Owner FLow Grant Type 

            var tokenClientRO = new TokenClient(Discovery.TokenEndpoint, "ro.client", "secret");
            var tokenResponseRO = await tokenClientRO.RequestResourceOwnerPasswordAsync("Kajol","password","bankApi");

            if (tokenResponseRO.IsError)
            {
                Console.WriteLine(tokenResponseRO.Error);
            }
            Console.WriteLine("\nResource Owner Flow\n\n");
            Console.WriteLine(tokenResponseRO.Json);
            Console.WriteLine("\n\n");

            //Consume our CLient APi
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var customerInfo = new StringContent(
                JsonConvert.SerializeObject(new { Id = 1, FirstName = "Lokesh", LastName = "Kashyap" }),
                Encoding.UTF8,"application/json");
            var createCustomerResponse = await client.PostAsync("https://localhost:44304/api/customers", customerInfo);

            if(!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerResponse.StatusCode);
            }

            var getCustomerResponse = await client.GetAsync("https://localhost:44304/api/customers");
            if(!getCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomerResponse.StatusCode);
            }
            else
            {
                var content = await getCustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.ReadKey();
        }
    }
}
