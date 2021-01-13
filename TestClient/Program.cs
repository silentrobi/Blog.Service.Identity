using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestClient
{
    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "Console Client Credentials Flow";

            var response = await RequestTokenAsync();
            Console.WriteLine(response.AccessToken);

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "blogapp",
                ClientSecret = "TYYvyjrJ4GThugsYGwnyEh2m63Tr7yyPk6YVwBb6"
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task CallServiceAsync(string token)
        {
            var baseAddress = Constants.SampleApi;

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);
            var response = await client.GetStringAsync("api/v1/test");

            Console.WriteLine(JArray.Parse(response));
        }
    }
}